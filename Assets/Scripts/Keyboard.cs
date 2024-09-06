using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Keyboard : MonoBehaviour
{
    public static Keyboard Instance { get; private set; }

    [SerializeField]
    private Button[] backSpace;
    [SerializeField]
    private Button[] numbersAndLetters;
    [SerializeField]
    private TMP_InputField usernameField;
    [SerializeField]
    private Button enterButton;
    [SerializeField]
    private Button capsLockButton;
    [SerializeField] private float spawnDistanceFromPlayer = 2f;

    [Space]

    public UnityEvent<string> OnEnter;

    private Transform head;

    private string enteredValue;
    private bool isCapsLockOn = true;

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
        enterButton.onClick.AddListener(Enter);

        foreach (Button item in backSpace)
        {
            item.onClick.AddListener(BackSpace);
        }

        foreach (Button item in numbersAndLetters)
        {
            item.onClick.AddListener(() => AddText(item.GetComponentInChildren<Text>().text));
        }

        capsLockButton.onClick.AddListener(() => CapsLockButtonPressed());
        CapsLockButtonPressed();

        head = Camera.main.transform;

        Toggle(false);
    }

    private void Enter()
    {

    }

    private void CapsLockButtonPressed()
    {
        isCapsLockOn = !isCapsLockOn;
        foreach (Button item in numbersAndLetters)
        {
            string letter = item.GetComponentInChildren<Text>().text.Trim();
            if (letter.Length == 1 && char.IsLetter(letter[0]))
            {
                item.GetComponentInChildren<Text>().text = isCapsLockOn ? letter.ToUpper() : letter.ToLower();
            }
        }

        if (isCapsLockOn)
        {
            capsLockButton.Select();
        }
    }

    private void BackSpace()
    {
        if (enteredValue.Length == 1)
        {
            enteredValue = "";
            return;
        }

        enteredValue = enteredValue[0..^1];
    }

    private void Update()
    {
        enterButton.interactable = !string.IsNullOrWhiteSpace(enteredValue);

        bool state = !string.IsNullOrWhiteSpace(enteredValue);
        foreach (Button item in backSpace)
        {
            item.interactable = state;
        }

        usernameField.text = enteredValue;
    }

    private void AddText(string txt)
    {
        enteredValue += txt;
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
