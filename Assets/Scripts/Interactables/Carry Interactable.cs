using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarryInteractable : Interactable
{
    // When the player interacts with this object, they are able to translate it and rotate it
    public Rigidbody rb;

    private bool isCarried = false;

    private Player player;

    private static float rotationSpeed = 7.5f; // Speed of rotation when carrying the object

    public FixedJoint carryJoint;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void Interact(Player player)
    {
        if (PlayerState.instance.currentState == PlayerStateType.CarryingObject
            && !isCarried)
        {
            Debug.Log("Player is already carrying an object.");
            return;
        }
        else if (isCarried)
        {
            DropObject(player);
            return;
        }

        PickUp(player);

    }

    private void DropObject(Player player)
    {
        Debug.Log("Player is dropping the carried object.");

        if (PlayerState.instance.currentState == PlayerStateType.RotatingCarryObject)
        {
            player.RotateCarryObject();
        }

            // Drop logic
        if (carryJoint != null)
        {
            Destroy(carryJoint); // Remove the joint
        }


        rb.useGravity = true;

        player.SetIsCarrying(false);
        isCarried = false;
        gameObject.layer = LayerMask.NameToLayer("Default");

        player.carriedObject = null;
    }

    private void PickUp(Player player)
    {
        // Pick up logic
        this.player = player;
        isCarried = true;
        player.SetIsCarrying(true);

        rb.useGravity = false;

        gameObject.layer = LayerMask.NameToLayer("Carry");

        carryJoint = gameObject.AddComponent<FixedJoint>();
        carryJoint.connectedBody = player.carryPoint.GetComponent<Rigidbody>();

        carryJoint.breakForce = Mathf.Infinity;
        carryJoint.breakTorque = Mathf.Infinity;

        this.player.carriedObject = this.gameObject;
    }

    public void EnableFixedJoint()
    {
        if (carryJoint == null)
        {
            carryJoint = gameObject.AddComponent<FixedJoint>();
            carryJoint.connectedBody = player.carryPoint.GetComponent<Rigidbody>();

            carryJoint.breakForce = Mathf.Infinity;
            carryJoint.breakTorque = Mathf.Infinity;

            player.carriedObject = this.gameObject;
        }

        
    }

    public void DisableFixedJoint()
    {
        if (carryJoint != null)
        {
            carryJoint.enableCollision = true;
            Destroy(carryJoint);
            carryJoint = null;
        }
    }

    public void RotateObject(InputAction.CallbackContext context)
    {
        if (context.performed && isCarried)
        {
            Vector2 rotationInput = context.ReadValue<Vector2>().normalized;
            Quaternion deltaRotation = Quaternion.Euler(rotationInput.y * rotationSpeed, rotationInput.x * rotationSpeed, 0f);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }
}
