using UnityEngine;
using UnityEngine.UI;

public class GraphicTogglesGroup : MonoBehaviour
{
    [SerializeField] private Toggle[] graphicsQualityToggles;

    [SerializeField] private GameSettingsSO gameSettingsSO;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Null check
        if (!gameSettingsSO)
        {
            gameSettingsSO = Resources.Load<GameSettingsSO>("Scriptable Objects/GameSettingsSO");
        }

        // Initialize UI
        for (int i = 0; i < graphicsQualityToggles.Length; i++)
        {
            graphicsQualityToggles[i].isOn = i == (int)gameSettingsSO.graphicsQuality;
        }

        for (int i = 0; i < graphicsQualityToggles.Length; i++)
        {
            int index = i; // Capture the current index
            graphicsQualityToggles[i].onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    SetGraphicsQuality((GameSettingsSO.GraphicsQuality)index);
                }
            });
        }
    }

    private void SetGraphicsQuality(GameSettingsSO.GraphicsQuality graphicsQuality)
    {
        gameSettingsSO.graphicsQuality = graphicsQuality;
        if (!graphicsQualityToggles[(int)graphicsQuality].isOn)
        {
            graphicsQualityToggles[(int)graphicsQuality].isOn = true;
        }
        // Change the quality settings in the game
        QualitySettings.SetQualityLevel((int)graphicsQuality);
    }
}
