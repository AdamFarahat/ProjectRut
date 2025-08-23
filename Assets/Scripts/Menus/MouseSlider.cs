using UnityEngine;
using UnityEngine.UI;

public class MouseSlider : MonoBehaviour
{
    [SerializeField] private Slider mouseSlider;

    [SerializeField] private GameSettingsSO gameSettingsSO;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Null check
        if (!gameSettingsSO)
        {
            gameSettingsSO = Resources.Load<GameSettingsSO>("Scriptable Objects/GameSettingsSO");
        }

        if (!mouseSlider)
        {
            mouseSlider = GetComponent<Slider>();
        }

        // Initialize UI
        mouseSlider.value = gameSettingsSO.mouseSensitivity;
        
        mouseSlider.onValueChanged.AddListener(SetMouseSensitivity);
    }

    private void SetMouseSensitivity(float value)
    {
        gameSettingsSO.mouseSensitivity = value;
        // Apply mouse sensitivity changes
        
    }
}
