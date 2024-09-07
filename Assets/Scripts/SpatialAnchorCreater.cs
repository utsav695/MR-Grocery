using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpatialAnchorCreater : MonoBehaviour
{
    public static SpatialAnchorCreater Instance { get; private set; }

    public GameObject productAnchorPrefab;

    private OVRSpatialAnchor lastSpatialAnchor;
    private ProductAnchorUIItem productUIBtn;
    private bool freeze;

    private Transform head;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);
        }

        Instance = this;
    }

    private void Start()
    {
        head = Camera.main.transform;
    }

    public void CreateSpatialAnchor(Vector3 position)
    {
        if (freeze)
        {
            return;
        }

        GameObject go = new();
        go.transform.SetPositionAndRotation(position,
            Quaternion.LookRotation((position - new Vector3(head.position.x, position.y, head.position.z)).normalized, Vector3.up));

        _ = StartCoroutine(CreateSpatialAnchor(go));
    }

    private IEnumerator CreateSpatialAnchor(GameObject go)
    {
        if (freeze)
        {
            yield return null;
        }

        freeze = true;

        OVRSpatialAnchor anchor = go.AddComponent<OVRSpatialAnchor>();

        yield return new WaitUntil(() => anchor.Created);

        Debug.Log($"Created anchor {anchor.Uuid}");

        lastSpatialAnchor = anchor;

        LoadAllProducts.Instance.Toggle(true);

        freeze = false;
    }

    public async void SaveAnchor(OVRSpatialAnchor anchor)
    {
        OVRResult<OVRAnchor.SaveResult> result = await anchor.SaveAnchorAsync();
        if (result.Success)
        {
            Debug.Log($"Anchor {anchor.Uuid} saved successfully.");

            Product product = productUIBtn.Product;

            if (!string.IsNullOrWhiteSpace(product.spatialAnchor))
            {
                ES3.Save(product.spatialAnchor, product.id, "id_map.es3");
            }

            Destroy(productUIBtn.gameObject);

            LoadAllProducts.Instance.Toggle(false);

            GameObject productAnchor = Instantiate(productAnchorPrefab, anchor.transform.position, anchor.transform.rotation);
            productAnchor.GetComponent<SpatialAnchorProduct>().Set(product);
        }
        else
        {
            Debug.LogError($"Anchor {anchor.Uuid} failed to save with error {result.Status}");
        }
    }

    public async void SaveAnchor(IEnumerable<OVRSpatialAnchor> anchors)
    {
        OVRResult<OVRAnchor.SaveResult> result = await OVRSpatialAnchor.SaveAnchorsAsync(anchors);
        if (result.Success)
        {
            Debug.Log($"Anchors saved successfully.");
        }
        else
        {
            Debug.LogError($"Failed to save {anchors.ToList().Count()} anchor(s) with error {result.Status}");
        }
    }

    public string GetLastAnchorUuid()
    {
        return lastSpatialAnchor.Uuid.ToString();
    }

    public void SaveLastCreatedAnchor(ProductAnchorUIItem productUIBtn)
    {
        SaveAnchor(lastSpatialAnchor);
        this.productUIBtn = productUIBtn;
    }
}
