using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    [SerializeField] private GameSettingsSO gameSettingsSO;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Null check
        if (!gameSettingsSO)
        {
            gameSettingsSO = Resources.Load<GameSettingsSO>("Scriptable Objects/GameSettingsSO");
        }

        if (!volumeSlider)
        {
            volumeSlider = GetComponent<Slider>();
        }

        // Initialize UI
        volumeSlider.value = gameSettingsSO.masterVolume;

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float value)
    {
        gameSettingsSO.masterVolume = value;
        AudioListener.volume = value; // Set the global audio volume
    }

}
