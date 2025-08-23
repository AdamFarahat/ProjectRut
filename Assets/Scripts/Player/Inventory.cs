using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    private int currentItemIndex = 0;

    public InventoryAction actions;

    private bool isInventoryOpen = false;

    public GameObject inventoryUI; // Reference to the inventory UI GameObject

    private TextMeshProUGUI itemNameText; // Reference to the TextMeshProUGUI for item name display 

    private Player player;

    private Pause pauseSystem;
    //Variables for item model preview
    public Transform itemPreviewSpawnPoint { get; private set; }
    public Camera inventoryCamera { get; private set; }

    //private RawImage itemPreviewImage; // Optional if you want to toggle visibility
    public GameObject currentItemPreview { get; private set; }

    [SerializeField] private AudioClip pickUpAudioClip;

    [SerializeField] private PlayerInventorySO playerInventorySO;

    PlayerStateType previousPlayerState;

    public void Awake()
    {
        actions = new InventoryAction();
        actions.Inventory.InventoryToggle.performed += _ => ToggleInventory();
        actions.Inventory.Next.performed += _ => Next();
        actions.Inventory.Previous.performed += _ => Previous();
        actions.Inventory.CycleItems.performed += CycleItems;
    }

    public void Start()
    {
        Transform InventoryPanelTransform = inventoryUI.transform.Find("Panel");
        GameObject panel = InventoryPanelTransform != null ? InventoryPanelTransform.gameObject : null;
        Transform itemNameTransform = panel != null ? panel.transform.Find("ItemName") : null;
        itemNameText = itemNameTransform != null ? itemNameTransform.GetComponent<TextMeshProUGUI>() : null;
        inventoryUI.SetActive(false);
        player = FindAnyObjectByType<Player>();

#if UNITY_EDITOR
        playerInventorySO.ClearItemsInstance();
#endif
        // Load info from PlayerInventorySO
        if (playerInventorySO != null)
        {
            items = playerInventorySO.items;
            currentItemIndex = playerInventorySO.currentItemIndex;
        }
        else
        {
            Debug.LogError("PlayerInventorySO not found in Resources");
        }

        actions.Disable();
        pauseSystem = FindAnyObjectByType<Pause>();
        inventoryCamera = transform.Find("InventoryCamera").gameObject?.GetComponent<Camera>();

        // find in children of InventoryCamera
        if (itemPreviewSpawnPoint == null)
        {
            itemPreviewSpawnPoint = inventoryCamera.transform.Find("ItemPreviewSpawnPoint").gameObject?.transform;
        }

    }

    public void OnEnable()
    {
        actions.Enable();
    }

    public void OnDisable()
    {
        actions.Disable();
    }

    public void ToggleInventory()
    {
        //Logic to toggle the inventory UI
        //unlock the cursor
        Debug.Log("Inventory toggled");
        if (isInventoryOpen)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }

    }

    private void OpenInventory()
    {
        if (PlayerState.instance.currentState == PlayerStateType.InMenu) return;
        previousPlayerState = PlayerState.instance.currentState;
        isInventoryOpen = true;
        // Show the inventory UI
        Debug.Log("Inventory opened");
        inventoryUI.SetActive(true);
        Cursor.visible = true; // Make the cursor visible
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        //disable player movement
        if (player != null)
        {
            player.playerInput.actions.Disable(); // Disable player input actions
        }

        //Set the item name text to the last item seen before closing the inventory
        if (items.Count > 0)
        {
            itemNameText.text = items[currentItemIndex].itemName; // Update the item name text
            ShowItemPreview();
        }
        else
        {
            itemNameText.text = ""; // Default text if no items
        }
        pauseSystem.action.Disable();


    }

    private void CloseInventory()
    {
        isInventoryOpen = false;
        // Hide the inventory UI
        if (currentItemPreview != null)
        {
            Destroy(currentItemPreview);
        }

        Debug.Log("Inventory closed");
        inventoryUI.SetActive(false);
        Cursor.visible = false; // Hide the cursor
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        if (player != null)
        {
            player.playerInput.actions.Enable(); // Re-enable player input actions
        }
        pauseSystem.action.Enable();
        PlayerState.instance.TriggerTransition(previousPlayerState);
    }

    public void CycleItems(InputAction.CallbackContext context)
    {
        if (!isInventoryOpen) return; // Only cycle items if the inventory is open
        int direction = Mathf.RoundToInt(context.ReadValue<Vector2>().y);
        if (items.Count != 0) currentItemIndex = (currentItemIndex + direction + items.Count) % items.Count;

        // Logic to cycle through items in the inventory
        if (items.Count > 0)
        {
            itemNameText.text = items[currentItemIndex].itemName; // Update the item name text
            ShowItemPreview();
        }

        Debug.Log("Current item index after cycling: " + currentItemIndex);
    }
    public void SetCurrentItemIndex(int index)
    {
        currentItemIndex = index;
        Debug.Log("Current item index set to: " + currentItemIndex);
    }

    public int GetCurrentItemIndex()
    {
        return currentItemIndex;
    }

    public void Next()
    {
        if (items.Count == 0 || !isInventoryOpen) return; // No items to cycle through
        currentItemIndex = (currentItemIndex + 1) % items.Count;
        playerInventorySO.currentItemIndex = currentItemIndex; // Update the current item index in the SO
        itemNameText.text = items[currentItemIndex].itemName; // Update the item name text
        Debug.Log("Next item selected: " + items[currentItemIndex].itemName);

        ShowItemPreview();
    }

    public void Previous()
    {
        if (items.Count == 0 || !isInventoryOpen) return; // No items to cycle through
        currentItemIndex = (currentItemIndex - 1 + items.Count) % items.Count;
        playerInventorySO.currentItemIndex = currentItemIndex; // Update the current item index in the SO
        itemNameText.text = items[currentItemIndex].itemName; // Update the item name text
        Debug.Log("Previous item selected: " + items[currentItemIndex].itemName);

        ShowItemPreview();
    }

    public void AddItem(string itemName, int itemKey, GameObject itemPrefab = null)
    {
        Item newItem = new(itemName, itemKey, itemPrefab);

        if (playerInventorySO != null)
        {
            playerInventorySO.items.Add(newItem);
            playerInventorySO.currentItemIndex = currentItemIndex;
            player.playerAudioSource.pitch = Random.Range(0.9f, 1.1f);
            player.playerAudioSource.PlayOneShot(pickUpAudioClip);
        }
        else
        {
            Debug.LogWarning("PlayerInventorySO is null, cannot update inventory SO.");
        }
    }

    public void RemoveItem()
    {
        if (items.Count > 0)
        {
            playerInventorySO.items.Remove(items[currentItemIndex]);
            playerInventorySO.currentItemIndex = 0;
            currentItemIndex = 0;
        }
    }

    public bool RemoveItemAtIndex(int ItemIndex)
    {
        if (items.Count > 0 && ItemIndex >= 0 && ItemIndex < items.Count)
        {
            items.RemoveAt(ItemIndex);
            currentItemIndex--;
            return true;
        }
        Debug.LogError("Wrong index passed to RemoveItemAtIndex");
        return false;
    }

    public int GetItemIndex(int itemKey)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemKey == itemKey)
            {
                Item FoundItem = items[i];
                return i;
            }
        }
        Debug.Log("Item not found");
        return -1;
    }

    public int GetEquippedItemKey()
    {
        if (items.Count > 0 && currentItemIndex >= 0 && currentItemIndex < items.Count)
        {
            return items[currentItemIndex].itemKey;
        }

        return -1;
    }

    public bool UseItemByItemKey(int itemKey)
    {
        int itemIndex = currentItemIndex;
        if (itemIndex >= 0)
        {
            if (RemoveItemAtIndex(itemIndex))
            {
                return true;
            }
        }
        return false;
    }

    public bool UseCurrentItem()
    {
        //TODO:
        //When the item is used, check if the interactable is not null
        //Check if the item key is the same as the itemKey of the interactable
        //if both checks pass, remove the item from the inventory and call the interactable's Interact method.
        return true;
    }

    private void ShowItemPreview()
    {
        if (itemPreviewSpawnPoint == null || items.Count == 0)
            return;

        // Destroy existing preview
        if (currentItemPreview != null)
            Destroy(currentItemPreview);

        Item currentItem = items[currentItemIndex];

        if (currentItem.itemPrefab != null)
        {
            currentItemPreview = Instantiate(currentItem.itemPrefab, itemPreviewSpawnPoint.position, Quaternion.identity, itemPreviewSpawnPoint);
            // Rotate 90 degress over Y and Z axis
            currentItemPreview.transform.localRotation = Quaternion.Euler(0, 90, 90);
            // Scale the preivew by 3 times
            currentItemPreview.transform.localScale = new Vector3(3, 3, 3);

            // Ensure it's on the correct layer so only the InventoryCamera sees it
            SetupChildrenRecursively(currentItemPreview, LayerMask.NameToLayer("ItemLayer"));
        }
    }

    // Utility function to set layer recursively
    private void SetupChildrenRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;
        // Disable collider and rigid body if they exist
        Collider collider = obj.GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
            rb.useGravity = false;
        foreach (Transform child in obj.transform)
        {
            if (child != null)
            {
                SetupChildrenRecursively(child.gameObject, newLayer);
            }
        }
    }

}
