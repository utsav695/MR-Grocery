using UnityEngine;

public class AnchorHand : MonoBehaviour
{
    private bool isRightHand;

    private void Start()
    {
        isRightHand = name.ToLower().Contains("right") || transform.parent.name.ToLower().Contains("right");

        if (isRightHand)
        {
            InputManager.RightController.SecondaryBtn.OnDown += PlaceAnchor;
        }
        else
        {
            InputManager.LeftController.SecondaryBtn.OnDown += PlaceAnchor;
        }
    }

    private void OnDestroy()
    {
        try
        {
            if (isRightHand)
            {
                InputManager.RightController.SecondaryBtn.OnDown -= PlaceAnchor;
            }
            else
            {
                InputManager.LeftController.SecondaryBtn.OnDown -= PlaceAnchor;
            }
        }
        catch (System.Exception)
        {

        }
    }

    private void PlaceAnchor()
    {
        //place anchor at transform.position

        if (Keypad.Instance)
        {
            Keypad.Instance.Toggle(true);
        }
        else if (Keyboard.Instance)
        {
            Keyboard.Instance.Toggle(true);
        }
    }
}
