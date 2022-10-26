using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private bool EnablePlayerInput = true;
    public Camera mainCamera;

    private Vector2 movementInput;
    private Vector3 movementTarget;
    private float tiltModifier = 0f;

    public float movementSpeed;

    private Vector3 lastPos;


    // Start is called before the first frame update
    void Start()
    {   playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Movement"].performed += Context => movementInput = Context.ReadValue<Vector2>();
        playerInput.actions["Movement"].canceled += Context => movementInput = Context.ReadValue<Vector2>();

        movementTarget = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();

        lastPos = this.transform.position;
    }

    private void HandleMovement() 
    {
        if (EnablePlayerInput)
        {
            // Movement
            if (movementInput.magnitude != 0)
            {
                movementTarget = Vector3.MoveTowards(movementTarget, mainCamera.ScreenToWorldPoint(new Vector3(movementInput.x, movementInput.y, 50f + transform.position.z)), 2f);
                float zMovementOffset = this.transform.position.z * 0.4f;
                movementTarget = new Vector3(Mathf.Clamp(movementTarget.x, -50f + -zMovementOffset, 50f + zMovementOffset), 0, Mathf.Clamp(movementTarget.z, 0f, 25f)); // Clamp player movement
            }
            // Move character
            this.transform.position = Vector3.Lerp(this.transform.position, movementTarget, movementSpeed);

            tiltModifier = movementTarget.x >= transform.position.x ? -1 : 1; // Set value based on movement direction (left/right)
            tiltModifier = Mathf.Abs(movementTarget.x - transform.position.x) < 25f ? 0 : tiltModifier; // Set minimum movement distance for tilting
            this.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 45f * tiltModifier), 0.75f);
        }
    }

    private float CalculateCharacterSpeed() 
    {
        Vector3 charSpeed = this.transform.position - lastPos;
        return charSpeed.magnitude;
    }

    private void SetPlayerInputEnabled(bool bEnable) 
    {
        EnablePlayerInput = bEnable;
    }
}
