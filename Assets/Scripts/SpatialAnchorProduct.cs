using UnityEngine;
using TMPro;

public class SpatialAnchorProduct : MonoBehaviour
{
    [SerializeField] private TextMeshPro idText;
    [SerializeField] private TextMeshPro nameText;

    private Product product;

    public void Set(Product product)
    {
        this.product = product;

        idText.text = product.id;
        nameText.text = product.name;
    }

    public string GetID()
    {
        if (product == null)
        {
            return string.Empty;
        }

        return product.id;
    }
}
