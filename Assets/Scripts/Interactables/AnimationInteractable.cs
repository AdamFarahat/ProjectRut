using UnityEngine;

public class AnimationInteractable : Interactable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public override void Interact(Player player)
    {
        // This method can be called to interact with the object
        Debug.Log("Interacted with " + gameObject.name+" by " + player.name);
    }
}
