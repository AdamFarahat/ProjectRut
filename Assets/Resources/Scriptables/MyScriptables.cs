using UnityEngine;

public abstract class MyScriptables : ScriptableObject
{
    public virtual void ClearAllData()
    {
        // This method can be overridden in derived classes to clear specific data
        Debug.Log("ClearAllData called on " + this.name);
    }
}
