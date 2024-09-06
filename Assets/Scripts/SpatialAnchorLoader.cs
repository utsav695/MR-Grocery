using System;
using System.Collections.Generic;
using UnityEngine;

public class SpatialAnchorLoader : MonoBehaviour
{
    // This reusable buffer helps reduce pressure on the garbage collector
    List<OVRSpatialAnchor.UnboundAnchor> _unboundAnchors = new();

    private void Start()
    {
        if (ES3.FileExists("id_list.txt"))
        {
            List<Guid> uuids = new();

            foreach (string id in ES3.LoadRawString("id_list.txt").Split('\n'))
            {
                if (ES3.KeyExists(id))
                {
                    Product product = ES3.Load<Product>(id);

                    if (!string.IsNullOrWhiteSpace(product.spatialAnchor))
                    {
                        uuids.Add(Guid.Parse(product.spatialAnchor));
                    }
                }
            }

            if (uuids.Count > 0)
            {
                LoadAnchorsByUuid(uuids);
            }
        }
    }

    private async void LoadAnchorsByUuid(IEnumerable<Guid> uuids)
    {
        // Step 1: Load
        var result = await OVRSpatialAnchor.LoadUnboundAnchorsAsync(uuids, _unboundAnchors);

        if (result.Success)
        {
            Debug.Log($"Anchors loaded successfully.");

            // Note result.Value is the same as _unboundAnchors
            foreach (var unboundAnchor in result.Value)
            {
                // Step 2: Localize
                unboundAnchor.LocalizeAsync().ContinueWith((success, anchor) =>
                {
                    if (success)
                    {
                        // Create a new game object with an OVRSpatialAnchor component
                        var spatialAnchor = new GameObject($"Anchor {unboundAnchor.Uuid}")
                            .AddComponent<OVRSpatialAnchor>();

                        // Step 3: Bind
                        // Because the anchor has already been localized, BindTo will set the
                        // transform component immediately.
                        unboundAnchor.BindTo(spatialAnchor);

                        string uuidString = spatialAnchor.Uuid.ToString();
                        if (ES3.KeyExists(uuidString, "id_map.es3"))
                        {
                            string id = ES3.Load<string>(uuidString, "id_map.es3");
                            if (ES3.KeyExists(id))
                            {
                                GameObject productAnchor = Instantiate(SpatialAnchorCreater.Instance.productAnchorPrefab,
                                    spatialAnchor.transform.position, spatialAnchor.transform.rotation);

                                productAnchor.GetComponent<SpatialAnchorProduct>().Set(ES3.Load<Product>(id));
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError($"Localization failed for anchor {unboundAnchor.Uuid}");
                    }
                }, unboundAnchor);
            }
        }
        else
        {
            Debug.LogError($"Load failed with error {result.Status}.");
        }
    }
}
