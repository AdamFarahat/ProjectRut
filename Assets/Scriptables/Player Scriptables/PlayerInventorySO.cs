using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerInventorySO", menuName = "Scriptable Objects/PlayerInventorySO")]
public class PlayerInventorySO : MyScriptables
{
    public int currentItemIndex = 0;

    public List<Item> items = new List<Item>();

    [Header("Item Prefab preference")]
    public List<SerializableKeyValuePair<int, GameObject>> itemList = new();

    [NaughtyAttributes.Button("Clear Items data")]
    public void ClearItemsInstance()
    {
        items.Clear();
        currentItemIndex = 0;
        Debug.Log("PlayerInventorySO items cleared.");
    }

    [NaughtyAttributes.Button("Clear All data")]
    override public void ClearAllData()
    {
        base.ClearAllData();
        items.Clear();
        currentItemIndex = 0;
        itemList.Clear();
    }
}


/* Serializable class for Item List */
[System.Serializable]
public class SerializableKeyValuePair<TKey, TValue>
{
    public TKey Key;
    public TValue Value;

    public SerializableKeyValuePair(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
}