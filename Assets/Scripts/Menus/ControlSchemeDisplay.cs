using UnityEngine;

public class ControlSchemeDisplay : MonoBehaviour
{
    [SerializeField] private GameObject PCControls;
    [SerializeField] private GameObject GamepadControls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Null check
        if (PCControls == null || GamepadControls == null)
        {
            Debug.LogError("Control scheme GameObjects are not assigned.");
            return;
        }

        SwapControlsDisplay();
    }

    [NaughtyAttributes.Button("Swap Controls Display")]
    private void SwapControlsDisplay()
    {
        if (GamepadControls.activeSelf)
        {
            PCControls.SetActive(true);
            GamepadControls.SetActive(false);
        }
        else
        {
            PCControls.SetActive(false);
            GamepadControls.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if on PC or console
        if (Input.GetJoystickNames().Length > 0)
        {
            // Gamepad is connected
            if (!GamepadControls.activeSelf)
            {
                SwapControlsDisplay();
            }
        }
        else
        {
            // No gamepad connected, assume PC controls
            if (!PCControls.activeSelf)
            {
                SwapControlsDisplay();
            }
        }
    }
}
