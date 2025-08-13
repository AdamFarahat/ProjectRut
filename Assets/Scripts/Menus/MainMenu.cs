using System.Collections;
using SojaExiles;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private Button start;

    private Button quit;

    [SerializeField] private GameObject door;

    private Player _player;

    private float distance;

    [SerializeField] private float cameraSpeed = 10f;

    [SerializeField] private GameObject playerEndLocation;

    private Pause pauseSystem;

    private Inventory inventorySystem;

    private GameObject blackVoid;

    public void Awake()
    {
        start = transform.Find("Start").gameObject?.GetComponent<Button>();
        if (start == null)
        {
            Debug.LogError("Start button not found");
        }

        start.onClick.AddListener(StartGame);

        quit = transform.Find("Quit").gameObject?.GetComponent<Button>();
        if (quit == null)
        {
            Debug.LogError("Quit button not found");
        }

        #if UNITY_WEBGL
                quit.SetActive(false);
        #endif

        quit.onClick.AddListener(QuitGame);

        _player = FindAnyObjectByType<Player>();

        pauseSystem = FindAnyObjectByType<Pause>();

        inventorySystem = FindAnyObjectByType<Inventory>();


    }

    public void Start()
    {
        PlayerState.instance.TriggerTransition(PlayerStateType.InMenu);
        StartCoroutine(MainMenuInit());

        blackVoid = GameObject.Find("BlackQuitVoid");

        blackVoid.SetActive(false);
    }

    private IEnumerator MainMenuInit()
    {
        yield return null;
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible
        EventSystem.current.SetSelectedGameObject(start.gameObject);
        _player.playerInput.enabled = false; // Disable player input
    }

    public void StartGame()
    {
        Debug.Log("Start Game");
        //Door opens as the camera slowly enters through the door. Entering the next room.
        //Load what is suppose to be behind the door, this case the Main Game Scene
        //Door Opens
        door.GetComponent<DoorAction>().OpenDoor();
        //camera moves until a certain point after the door
        StartCoroutine(StartSceneTransition(_player.transform.position, playerEndLocation.transform.position, cameraSpeed));
        //Load the Game Scene
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        blackVoid.SetActive(true);
        door.GetComponent<DoorAction>().OpenDoor();
        StartCoroutine(QuitSceneTransition(_player.transform.position, playerEndLocation.transform.position, cameraSpeed));
    }

    public void Update()
    {
        if (EventSystem.current != null)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                if (EventSystem.current.GetComponent<InputSystemUIInputModule>().move.action.triggered)
                {
                    EventSystem.current.SetSelectedGameObject(start.gameObject);
                }
            }
        }
    }

    private IEnumerator StartSceneTransition(Vector3 playerStartPos, Vector3 playerEndPos, float timeToReach)
    {
        start.interactable = false;
        quit.interactable = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        float elapsed = 0f;
        while (elapsed < timeToReach)
        {
            _player.transform.position = Vector3.Lerp(playerStartPos, playerEndPos, elapsed / timeToReach);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _player.transform.position = playerEndPos;

        door.GetComponent<DoorAction>().CloseDoor();
        _player.playerInput.enabled = true;
        PlayerState.instance.TriggerTransition(PlayerStateType.Idle);
        this.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; // Unlock the cursor
        Cursor.visible = false; // Make the cursor visible
        EventSystem.current.SetSelectedGameObject(null);
        //Unlock other systems
        pauseSystem.action.Enable();
        inventorySystem.actions.Enable();

    }

    private IEnumerator QuitSceneTransition(Vector3 playerStartPos, Vector3 playerEndPos, float timeToReach)
    {
        start.interactable = false;
        quit.interactable = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        float elapsed = 0f;
        while (elapsed < timeToReach)
        {
            _player.transform.position = Vector3.Lerp(playerStartPos, playerEndPos, elapsed / timeToReach);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _player.transform.position = playerEndPos;

        Application.Quit();

    }
}
