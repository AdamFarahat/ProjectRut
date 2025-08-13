using System.Collections;
using UnityEngine;

public class EndRoom : MonoBehaviour
{
    [SerializeField] private Player player;

    [SerializeField] private Transform playerCamera;

    [SerializeField] private GameObject demon;

    [SerializeField] private Transform playerSpawnPosition;

    [SerializeField] private Transform retreatPosition;

    [SerializeField] private Door exitDoor;

    private AudioSource jumpScareSource;

    [SerializeField] private AudioClip jumpScareClip;

    private bool isDemonRetreating = false;

    public static EndRoom instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (player == null)
        {
            player = FindAnyObjectByType<Player>();
        }

        if (demon == null)
        {
            Debug.LogError("Demon GameObject is not assigned in the EndRoom script.");
        }

        if (exitDoor == null)
        {
            Debug.LogError("Exit Door is not assigned in the EndRoom script.");
        }
    }

    void Start()
    {
        jumpScareSource = gameObject.GetComponent<AudioSource>();
        jumpScareSource.spatialBlend = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        if (isDemonRetreating)
        {
            if (demon != null && retreatPosition != null)
            {
                demon.transform.position = Vector3.MoveTowards(demon.transform.position, retreatPosition.position, Time.deltaTime * 2f);
                if (Vector3.Distance(demon.transform.position, retreatPosition.position) < 5.0f)
                {
                    if (exitDoor != null)
                    {
                        exitDoor.CloseDoorInspector();
                    }
                }

                if (Vector3.Distance(demon.transform.position, retreatPosition.position) < 0.1f)
                {
                    isDemonRetreating = false;
                    demon.SetActive(false);
                    // Reload this scene
                    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                }
            }
        }
    }

    [NaughtyAttributes.Button("Start End Room Event")]
    public void StartEndRoomEvent()
    {
        if (player != null && demon != null)
        {
            // Disable player controls

            jumpScareSource.pitch = Random.Range(0.95f, 1.1f);
            jumpScareSource.PlayOneShot(jumpScareClip);

            player.playerInput.actions.Disable();
            player.playerInput.enabled = false;

            player.transform.position = playerSpawnPosition.position;
            player.transform.rotation = playerSpawnPosition.rotation;

            // Set player camera to look at the demon
            playerCamera.transform.LookAt(retreatPosition);

            demon.SetActive(true);

            exitDoor.OpenDoorInspector();
            StartCoroutine(DemonRetreatCoroutine(5f));
        }
        else
        {
            Debug.LogError("Player or Demon is not assigned in the EndRoom script.");
        }
    }

    private IEnumerator DemonRetreatCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        isDemonRetreating = true;
    }
}
