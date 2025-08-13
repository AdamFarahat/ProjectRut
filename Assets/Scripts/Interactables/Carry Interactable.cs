using Unity.VisualScripting;
using UnityEngine;

public class CarryInteractable : Interactable
{
    // When the player interacts with this object, they are able to translate it and rotate it
    Rigidbody rb;

    private bool isCarried = false;

    private Player player;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void Interact(Player player)
    {
        if (player.isCarrying && !isCarried)
        {
            Debug.Log("Player is already carrying an object.");
            return;
        }
        else if (isCarried)
        {
            Debug.Log("Player is dropping the carried object.");
            rb.isKinematic = false; // Re-enable physics so the object can fall
            player.SetIsRotatingCarryObject(false);
            player.SetIsCarrying(false);
            isCarried = false;
            player.OnToggleCarryRotation(); // Reset the rotation toggle
            return;
        }
        player.SetIsCarrying(true);
        //Have the player pick up the object
        rb.isKinematic = true; // Disable physics so the object can be moved manually
        //Get the raycast the player is using to interact with objects

        //The item will be floating in front of the player based on the position of the raycast hit
        transform.position = player.carryPoint.position;

        this.player = player;
        isCarried = true;
    }

    public void RotateCarryObject(Vector2 lookInput)
    {
        if (!player.isRotatingCarryObject) return;

        Debug.Log("lookInput: " + lookInput);

        lookInput = lookInput.normalized; // Normalize the input to prevent speed increase with larger input values

        Vector3 rotation = new Vector3(lookInput.y, lookInput.x, 0);
        transform.rotation *= Quaternion.Euler(rotation);
    }


    public void Update()
    {
        if (isCarried)
        {
            // Update the position of the carried object to follow the player
            transform.position = player.carryPoint.position;
        }
    }
}
