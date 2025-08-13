using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    TextMeshProUGUI interactPrompt;

    public GameObject endGamePanel{ get; private set; }

    public GameObject endGameVolume { get; private set; }

    private Button quitGame;

    Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        player = GameObject.Find("Player").GetComponent<Player>();
        interactPrompt = transform.Find("Interact Prompt").GetComponent<TextMeshProUGUI>();
        if (interactPrompt == null)
        {
            Debug.LogError("Interact Prompt TextMeshProUGUI not found in HUD.");
        }
        else
        {
            ShowInteractPrompt(false);
            interactPrompt.text = "Press to interact";
        }

        endGamePanel = transform.Find("EndGame")?.gameObject;

        if (quitGame != null)
        {
#if UNITY_WEBGL
            quitGame.gameObject.SetActive(false); // Hide on WebGL
#endif
        }


        endGamePanel.SetActive(false);

        endGameVolume = GameObject.Find("End Game Volume");
    }

    public void QuitGame() {
        Debug.Log("QuitGame");
        Application.Quit();
    }

    public void ShowInteractPrompt(bool show)
    {
        if (interactPrompt != null)
        {
            interactPrompt.gameObject.SetActive(show);
        }
    }

    public void ShowEndGamePrompt()
    {
        player.playerInput.enabled = false;
        PlayerState.instance.TriggerTransition(PlayerStateType.InMenu);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        endGamePanel.SetActive(true);
        if (quitGame != null)
        {
        #if UNITY_WEBGL
            quitGame.gameObject.SetActive(false); // Hide on WebGL
            EventSystem.current.SetSelectedGameObject(quitGame.gameObject);
        #endif
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
