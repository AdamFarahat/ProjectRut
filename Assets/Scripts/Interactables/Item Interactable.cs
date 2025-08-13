using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemInteractable : Interactable
{

    private Item item;

    [SerializeField] private string itemName;
    [SerializeField] private int itemKey;

    [SerializeField] private PlayerInventorySO playerInventorySO;

    [SerializeField] private GameObject itemPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        item = new Item(itemName, itemKey, itemPrefab);
        Debug.Log("Item created: " + item.itemName + " with key: " + item.itemKey);
    }

    public override void Interact(Player player)
    {
        if (player == null)
        {
            Debug.LogError("Player is null, cannot interact with item.");
            return;
        }
        else if (!player.inventory)
        {
            Debug.LogError("Player's inventory is null, cannot add item.");
            return;
        }
        else
        {
            player.inventory.AddItem(item.itemName, item.itemKey, GetItemPrefab(item.itemKey));
            // player.inventory.AddItem(item.itemName, item.itemKey, item);
            Destroy(gameObject);
        }
    }

    // Get the item prefab from the playerInventorySO
    public GameObject GetItemPrefab(int itemKey)
    {
        if (playerInventorySO != null)
        {
            foreach (var pair in playerInventorySO.itemList)
            {
                if (pair.Key == itemKey)
                {
                    return pair.Value;
                }
            }
        }
        Debug.LogWarning("Item prefab not found for key: " + itemKey);
        return null;
    }
}
