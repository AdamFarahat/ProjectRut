using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    [SerializeField] private GameSettingsSO gameSettingsSO;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Null check
        if (!gameSettingsSO)
        {
            gameSettingsSO = Resources.Load<GameSettingsSO>("Scriptable Objects/GameSettingsSO");
        }

        if (!resolutionDropdown)
        {
            resolutionDropdown = GetComponent<TMP_Dropdown>();
        }

        // Screen class to populate the dropdown
        resolutionDropdown.ClearOptions();
        PopulateResolutionDropdown();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    private void PopulateResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();

        foreach (GameSettingsSO.Resolution resolution in System.Enum.GetValues(typeof(GameSettingsSO.Resolution)))
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.ToString()[2..]));
        }

        resolutionDropdown.value = (int)gameSettingsSO.resolution;
    }

    private void SetResolution(int index)
    {
        // Set the resolution based on the dropdown index
        GameSettingsSO.Resolution selectedResolution = (GameSettingsSO.Resolution)System.Enum.GetValues(typeof(GameSettingsSO.Resolution)).GetValue(index);
        gameSettingsSO.resolution = selectedResolution;
        // split string
        string[] resolutionParts = selectedResolution.ToString().Split('_');
        if (resolutionParts.Length == 2)
        {
            int width = int.Parse(resolutionParts[1].Split('x')[0]);
            int height = int.Parse(resolutionParts[1].Split('x')[1]);
            Screen.SetResolution(width, height, gameSettingsSO.isFullscreen);
        }
    }
}
