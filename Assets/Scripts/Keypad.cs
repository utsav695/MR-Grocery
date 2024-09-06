using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Keypad : MonoBehaviour
{
    public static Keypad Instance { get; private set; }

    [SerializeField] private Button[] keys;
    [SerializeField] private Button enter;

    [Space]

    [SerializeField] private TextMeshProUGUI preview;

    [Space]

    public UnityEvent<string> OnEnter;

    [Space]

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
        Erase();

        foreach (Button key in keys)
        {
            key.onClick.AddListener(() => Keypress(key.GetComponentInChildren<TextMeshProUGUI>().text));
        }

        enter.onClick.AddListener(Enter);

        head = Camera.main.transform;

        Toggle(false);
    }

    private void Update()
    {
        enter.interactable = !string.IsNullOrWhiteSpace(preview.text);

    }

    private void Keypress(string key)
    {
        preview.text += key;
    }

    private void Enter()
    {
        OnEnter.Invoke(preview.text);
        Erase();
    }

    public void Erase()
    {
        preview.text = string.Empty;
    }

    public void Toggle(bool state)
    {
        transform.GetChild(0).gameObject.SetActive(state);

        if (!state)
        {
            return;
        }

        Vector3 forwardDirection = head.forward;
        forwardDirection.y = 0;
        forwardDirection.Normalize();

        transform.position = head.position + (forwardDirection * spawnDistanceFromPlayer);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, head.eulerAngles.y,
            transform.localEulerAngles.z);
    }

    public bool IsActive()
    {
        return transform.GetChild(0).gameObject.activeInHierarchy;
    }
}
