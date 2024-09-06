using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductAnchorUIItem : MonoBehaviour
{
    public Product Product { get; private set; }

    [SerializeField] private TextMeshProUGUI idText;
    [SerializeField] private TextMeshProUGUI nameText;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(Select);
    }

    private void Select()
    {
        if (Product == null)
        {
            return;
        }

        Product.spatialAnchor = SpatialAnchorCreater.Instance.GetLastAnchorUuid();

        if (string.IsNullOrWhiteSpace(Product.spatialAnchor))
        {
            return;
        }

        ES3.DeleteKey(Product.id);
        ES3.Save(Product.id, Product);

        SpatialAnchorCreater.Instance.SaveLastCreatedAnchor(this);
    }

    public void Set(Product product)
    {
        Product = product;

        idText.text = product.id;
        nameText.text = product.name;
    }
}
