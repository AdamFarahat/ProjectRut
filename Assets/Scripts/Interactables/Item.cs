using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Item
{
    public string itemName { get; private set; }

    public int itemKey { get; private set; }

    public GameObject itemPrefab{ get; private set; }

    //public Texture2D inventoryImage { get; private set; }

    public Item(string name, int key, GameObject prefab = null)
    {
        itemName = name;
        itemKey = key;
        itemPrefab = prefab;
    }
}
