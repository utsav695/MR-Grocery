using UnityEngine;

public class RayUiSelectorToggle : MonoBehaviour
{
    [SerializeField] private GameObject[] rays;

    private bool activeState;

    private void Update()
    {
        activeState = (Keypad.Instance && Keypad.Instance.IsActive()) || (Keyboard.Instance && Keyboard.Instance.IsActive());

        foreach (GameObject ray in rays)
        {
            ray.SetActive(activeState);
        }
    }
}
