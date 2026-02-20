using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8f;     
    public float jumpForce = 13f;    
    public float gravityMultiplier = 4f; 
    public LayerMask groundLayer;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;
    private float distToGround;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f, groundLayer);

        Vector3 extraGravity = Physics.gravity * (gravityMultiplier - 1f);
        rb.AddForce(extraGravity, ForceMode.Acceleration);

        bool wallRight = Physics.Raycast(transform.position, Vector3.right, 0.6f, groundLayer);
        bool wallLeft = Physics.Raycast(transform.position, Vector3.left, 0.6f, groundLayer);

        float finalMove = moveInput.x;
        if (wallRight && moveInput.x > 0) finalMove = 0;
        if (wallLeft && moveInput.x < 0) finalMove = 0;

        rb.linearVelocity = new Vector3(finalMove * moveSpeed, rb.linearVelocity.y, 0f);
    }
}