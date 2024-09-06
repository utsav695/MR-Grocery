using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class ProductLookUpManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private TextMeshProUGUI tagsField;

    [SerializeField] private Button searchButton;

    private List<Product> products;

    private void Start()
    {
        searchButton.onClick.AddListener(Search);

        products = new();

        if (ES3.FileExists("id_list.txt"))
        {
            foreach (string id in ES3.LoadRawString("id_list.txt").Split('\n').ToList())
            {
                if (ES3.KeyExists(id))
                {
                    products.Add(ES3.Load<Product>(id));
                }
            }
        }
    }

    private void Search()
    {
        List<string> tags = tagsField.text.Split(',').ToList();
        tags.ForEach(t => t.Trim().ToLower());

        string productName = nameField.text.Trim().ToLower();

        Product foundProduct = null;
        int maxTagMatches = 0;

        foreach (Product product in products)
        {
            if (product.name.Trim().ToLower() == productName)
            {
                int currentTagMatches = 0;
                foreach (string tag in tags)
                {
                    if (product.tags.Contains(tag))
                    {
                        currentTagMatches++;
                    }
                }

                if (currentTagMatches > maxTagMatches)
                {
                    foundProduct = product;
                    maxTagMatches = currentTagMatches;
                }
            }
        }
    }

    private void Update()
    {
        searchButton.interactable = !string.IsNullOrEmpty(nameField.text.Trim()) && !string.IsNullOrEmpty(nameField.text.Trim());
    }
}
