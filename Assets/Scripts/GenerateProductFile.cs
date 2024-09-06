using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class GenerateProductFile : MonoBehaviour
{
    [SerializeField] private TMP_InputField idField;
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField tagsField;

    [Space]

    [SerializeField] private GameObject logUi;
    [SerializeField] private TextMeshProUGUI logMessage;
    [SerializeField] private float messageVisibleDuration = 3f;

    [Space]

    [SerializeField] private Button createButton;

    private float logUiTimer;

    private void Start()
    {
        createButton.onClick.AddListener(Create);
    }

    private void Update()
    {
        createButton.interactable = !string.IsNullOrWhiteSpace(idField.text) && !string.IsNullOrWhiteSpace(nameField.text) &&
            !string.IsNullOrWhiteSpace(tagsField.text);

        if (logUiTimer > 0f)
        {
            logUiTimer -= Time.deltaTime;
            logUi.SetActive(true);
        }
        else
        {
            logUi.SetActive(false);
        }
    }

    private void Create()
    {
        string id = idField.text.Trim();

        if (ES3.KeyExists(id))
        {
            logMessage.text = "ID already exists";
            idField.text = string.Empty;
            logUiTimer = messageVisibleDuration;
            return;
        }

        List<string> tags = tagsField.text.Split(',').ToList();
        tags.ForEach(t => t.Trim().ToLower());

        ES3.Save(id, new Product(id, nameField.text.Trim(), tags));
        ES3.AppendRaw(id + "\n", "id_list.txt");

        idField.text = string.Empty;
        nameField.text = string.Empty;
        tagsField.text = string.Empty;
    }
}
