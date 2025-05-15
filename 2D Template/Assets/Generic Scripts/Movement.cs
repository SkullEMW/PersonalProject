using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class Movement : MonoBehaviour
{
    [Header("General")]
    bool isFacingRight = true;

    [Header("Movement")]
    public float movementSpeed;
    private Rigidbody2D _rb;
    private Vector2 _moveAmount;
    private float dashDirection;
    public bool isMoving;

    private float exe;
    float horizontalMove = 0f;
    float verticalMove = 0f;
    float _move = 0f;


    [Header("Dash/Sprint")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 0.1f;
    public bool isDashing;
    public bool canDash = true;
    TrailRenderer _trailRenderer;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.linearVelocity = _moveAmount * movementSpeed;

        horizontalMove = Input.GetAxisRaw("Horizontal") * movementSpeed;
        verticalMove = Input.GetAxisRaw("Vertical") * movementSpeed;

        _move = horizontalMove + verticalMove;
    }

    public void HandleMovement(InputAction.CallbackContext context)
    {
        _moveAmount = context.ReadValue<Vector2>();

        if (context.performed)
        {
            isMoving = true;
            
        }
        else
        {
            isMoving = false;

        }

    }


    public void HandleDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    

    private IEnumerator DashCoroutine()
    {
        Physics2D.IgnoreLayerCollision(3, 8, true);
        canDash = false;
        isDashing = true;

        _trailRenderer.emitting = true;
        dashDirection = !isFacingRight ? -1f : 1f;

        Vector2 dash = _rb.position;

        dash += _moveAmount * dashSpeed;

        _rb.MovePosition(dash);  // Dash Movement

        yield return new WaitForSeconds(dashDuration);

        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y); // Resets the velocity

        isDashing = false;
        _trailRenderer.emitting = false;
        Physics2D.IgnoreLayerCollision(8, 3, false); // Allows the player to dash through without taking damage.

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;


    }
}
