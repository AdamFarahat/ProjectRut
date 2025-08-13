using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemKeyToSceneNameSO", menuName = "Scriptable Objects/ItemKeyToSceneNameSO")]
public class ItemKeyToSceneNameSO : MyScriptables
{
    public Dictionary<int, string> itemKeyToSceneName = new()
    {
        // Insert the rest of the key item keys here alongside the scene name they are supposed to open
        {101, "Scene 1"}
    };

    [NaughtyAttributes.Button("Clear All data")]
    override public void ClearAllData()
    {
        base.ClearAllData();
        itemKeyToSceneName.Clear();
    }
}
