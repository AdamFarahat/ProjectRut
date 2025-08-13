using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Vector2 movementInput;

    [SerializeField] float movementSpeed = 5f;
    [SerializeField] bool isInverted = false;

    GameObject _camera;
    private float xRotation = 0f;
    private float yRotation = 0f;

    Rigidbody rb;

    // Clamp angles for vertical look (Y-axis rotation)
    [SerializeField] private float minY = -40f;  // Min vertical rotation angle
    [SerializeField] private float maxY = 40f;   // Max vertical rotation angle

    private float currentXRotation = 0f;  // Track current pitch (vertical rotation)
    
    [SerializeField] private float mouseSensitivity = 100f; // Mouse sensitivity multiplier

    [SerializeField] private float gamepadSensitivity = 100f; // Gamepad analog stick sensitivity multiplier

    public PlayerInput playerInput { get; private set; }

    private RaycastHit hit;

    public Inventory inventory{get; private set;}

    public bool isCarrying { get; private set; } = false;

    public bool isRotatingCarryObject { get; private set; } = false;

    public GameObject carriedObject;

    public Transform carryPoint;

    [SerializeField] private float hitRange = 2f;

    HUD HUD;

    public void SetIsCarrying(bool result)
    {
        isCarrying = result;
        //Add controls for rotating the carried object
    }

    public void SetIsRotatingCarryObject(bool result)
    {
        isRotatingCarryObject = result;
    }

    public RaycastHit GetRaycastHit()
    {
        return hit;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        
        Vector2 lookInput = context.ReadValue<Vector2>();

        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
        {
            // Analog stick sensitivity multiplier
            xRotation = lookInput.x * gamepadSensitivity; // Tweak multiplier as needed
            yRotation = lookInput.y * gamepadSensitivity;
        }
        else
        {
            // Mouse input (already in delta)
            xRotation = lookInput.x * mouseSensitivity;
            yRotation = lookInput.y * mouseSensitivity;
        }
    }

    public void OnInteract()
    {
        Debug.Log("Interact button pressed");
        if (hit.collider!= null && hit.collider.gameObject.GetComponent<Interactable>() != null)
        {
            // Call the Interact method on the Interactable component
            hit.collider.GetComponent<Interactable>().Interact(this);
        }
    }

    public void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;
        playerInput.actions["Look"].performed += OnLook;
        playerInput.actions["Look"].canceled += OnLook;
        //playerInput.actions["Scroll"].performed += inventory.CycleItems;

        playerInput.actions["Interact"].performed += ctx => OnInteract();
        playerInput.actions["RotateCarryObject"].performed += ctx => ToggleCarryRotation();
    }

    public void ToggleCarryRotation()
    {
        if (isCarrying)
        {
            isRotatingCarryObject = !isRotatingCarryObject;
            OnToggleCarryRotation();
        }
        
    }

    private System.Action<InputAction.CallbackContext> rotateCarryObjectCallback;

    public void OnToggleCarryRotation()
    {
        if (isCarrying)
        {
            Debug.Log("Toggling Carry Rotation: " + isRotatingCarryObject);
            if (isRotatingCarryObject)
            {
                // Unsubscribe from normal camera look
                playerInput.actions["Look"].performed -= OnLook;

                // Only set up the callback once
                if (rotateCarryObjectCallback == null)
                {
                    rotateCarryObjectCallback = ctx =>
                    {
                        if (carriedObject != null)
                        {
                            carriedObject.GetComponent<CarryInteractable>().RotateCarryObject(ctx.ReadValue<Vector2>());
                        }
                    };
                }

                // Make sure carriedObject is updated
                if (hit.collider != null && hit.collider.gameObject.TryGetComponent(out CarryInteractable carryInteractable))
                {
                    carriedObject = hit.collider.gameObject;
                    playerInput.actions["Look"].performed += rotateCarryObjectCallback;
                }
            }
            else
            {
                Debug.Log("Current Carried Object: " + carriedObject);
                playerInput.actions["Look"].performed -= rotateCarryObjectCallback;
                playerInput.actions["Look"].performed += OnLook;
            }
            return;
        }
        playerInput.actions["Look"].performed -= rotateCarryObjectCallback;
        playerInput.actions["Look"].performed += OnLook;
    }



    void Start()
    {
        HUD = GameObject.Find("UI").transform.Find("HUD").GetComponent<HUD>();
        if (HUD == null)
        {
            Debug.LogError("HUD not found");
        }
        Cursor.lockState = CursorLockMode.Locked;
        _camera = GameObject.Find("camera");
        rb = GetComponent<Rigidbody>();

        // Disable Rigidbody rotation so manual rotation doesn't conflict
        rb.freezeRotation = true;

        // Initialize camera rotation
        _camera.transform.localRotation = Quaternion.Euler(0, 0, 0);

        hit = new RaycastHit();

        //Connect to Inventory System
        inventory = FindAnyObjectByType<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("Inventory System not found in Hierarchy");
        }

        //Intialize carry point
        carryPoint = new GameObject("CarryPoint").transform;
        carryPoint.SetParent(_camera.transform);
        carryPoint.localPosition = new Vector3(0, 0, 2f);
    }

    void Update()
    {
        // Draw a ray for debugging purposes
        Debug.DrawRay(_camera.transform.position, _camera.transform.forward * hitRange, Color.red);
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, hitRange))
        {
            //Check if the object has an Interactable component, show UI prompt to tell the player they can interact.
            if (hit.collider.gameObject.TryGetComponent(out Interactable interactable))
            {
                HUD.ShowInteractPrompt(true);
                //Debug.Log("Press 'E' to interact");
            }
        }
        else
        {
            HUD.ShowInteractPrompt(false);
        }
    }

    void FixedUpdate()
    {
        // Movement
        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        transform.Translate(moveDirection * movementSpeed * Time.deltaTime);
        // Horizontal rotation (Player body)
        transform.Rotate(Vector3.up * xRotation * Time.deltaTime);

        // Vertical rotation (Camera)
        if (!isInverted)
        {
            currentXRotation -= yRotation * Time.deltaTime;  // Inverted vertical rotation
        }
        else
        {
            currentXRotation += yRotation * Time.deltaTime;  // Normal vertical rotation
        }

        // Clamp the vertical rotation to the desired limits
        currentXRotation = Mathf.Clamp(currentXRotation, minY, maxY);

        // Apply the clamped vertical rotation to the camera
        _camera.transform.localRotation = Quaternion.Euler(currentXRotation, 0, 0);
    }
}
