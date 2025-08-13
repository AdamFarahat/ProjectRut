using UnityEngine;

[RequireComponent(typeof(DoorAction))]
[RequireComponent(typeof(DoorInteractable))]
public class Door : MonoBehaviour
{
    Player player;
    public DoorAction doorAction;

    [Header("Door Settings")]
    public bool isInteractable = false;
    [NaughtyAttributes.ShowIf("isInteractable")]
    public DoorInteractable doorInteractable;

    [NaughtyAttributes.ShowIf("isInteractable")]
    [Tooltip("If true, the door will teleport the player to the teleport target instead of opening.\n" +
             "Only makes sense if the door is interactable.")]
    public bool isTeleportable = false;
    [NaughtyAttributes.ShowIf("isTeleportable")]
    public Transform teleportTarget;

    private GameObject closeDoorVolume;

    private GameObject openDoorVolume;

    private void Awake()
    {
        doorAction = GetComponent<DoorAction>();
        if (doorAction == null)
        {
            Debug.LogError("DoorAction component not found on the Door object.");
        }

        if (isInteractable)
        {
            doorInteractable = GetComponent<DoorInteractable>();
            if (doorInteractable == null)
                Debug.LogError("DoorInteractable component not found on the Door object.");
        }
    }

    private void Start()
    {
        closeDoorVolume = transform.parent.Find("DoorCloseVolume")?.gameObject;

        openDoorVolume = transform.parent.Find("DoorOpenVolume")?.gameObject;
    }

    // Make a function to open the door in the inspector
    [NaughtyAttributes.Button("Open Door")]
    public void OpenDoorInspector()
    {
        doorAction.OpenDoor();
    }

    // Make a function to close the door in the inspector
    [NaughtyAttributes.Button("Close Door")]
    public void CloseDoorInspector()
    {
        doorAction.CloseDoor();
    }

    // Make a function to teleport the player to the door's teleport target
    [NaughtyAttributes.Button("Teleport Player")]
    public void TeleportPlayerInspector()
    {
        if (player != null && teleportTarget != null)
        {
            player.transform.position = teleportTarget.position;
            Debug.Log("Player teleported to " + teleportTarget.name);
        }
        else
        {
            Debug.LogWarning("Player or teleport target is not set.");
        }
    }

    public bool CheckIfDoorIsOpened()
    {
        if (doorAction != null)
        {
            return doorAction.open;
        }
        else
        {
            Debug.LogError("DoorAction component is not assigned.");
            return false;
        }
    }

    public void Update()
    {
        if (closeDoorVolume != null && CheckIfDoorIsOpened())
        {
            Collider[] closeColliders = Physics.OverlapBox(
                closeDoorVolume.transform.position,
                closeDoorVolume.transform.localScale / 2,
                closeDoorVolume.transform.rotation
            );

            foreach (var collider in closeColliders)
            {
                Player collidedPlayer = collider.GetComponent<Player>();
                if (collidedPlayer != null)
                {
                    doorAction.CloseDoor();
                    break;
                }
            }
        }

        if (openDoorVolume != null && !CheckIfDoorIsOpened())
            {
                Collider[] openColliders = Physics.OverlapBox(
                    openDoorVolume.transform.position,
                    openDoorVolume.transform.localScale / 2,
                    openDoorVolume.transform.rotation
                );

                foreach (var collider in openColliders)
                {
                    Player collidedPlayer = collider.GetComponent<Player>();
                    if (collidedPlayer != null)
                    {
                        doorAction.OpenDoor();
                        break;
                    }
                }
            }
    }
}
