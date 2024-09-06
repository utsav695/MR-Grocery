using UnityEngine;

public class LoadAllProducts : MonoBehaviour
{
    public static LoadAllProducts Instance { get; private set; }

    [SerializeField] private GameObject uiPrefab;
    [SerializeField] private Transform uiParent;
    [SerializeField] private Transform canvas;
    [SerializeField] private float spawnDistanceFromPlayer = 2f;

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
        Toggle(false);

        if (ES3.FileExists("id_list.txt"))
        {
            foreach (string id in ES3.LoadRawString("id_list.txt").Split('\n'))
            {
                if (ES3.KeyExists(id))
                {
                    Product product = ES3.Load<Product>(id);

                    if (string.IsNullOrWhiteSpace(product.spatialAnchor))
                    {
                        GameObject ui = Instantiate(uiPrefab, uiParent);
                        ui.GetComponent<ProductAnchorUIItem>().Set(product);
                    }
                }
            }
        }

        head = Camera.main.transform;
    }

    public void Toggle(bool state)
    {
        canvas.gameObject.SetActive(state);

        if (!state)
        {
            return;
        }

        Vector3 forwardDirection = head.forward;
        forwardDirection.y = 0;
        forwardDirection.Normalize();

        canvas.position = head.position + (forwardDirection * spawnDistanceFromPlayer);
        canvas.localEulerAngles = new Vector3(canvas.localEulerAngles.x, head.eulerAngles.y,
            canvas.localEulerAngles.z);
    }

    public int GetUnanchoredProductsCount()
    {
        return uiParent.childCount;
    }
}
