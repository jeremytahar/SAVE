using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float jumpForce = 13f;
    public float gravityMultiplier = 4f;
    public LayerMask groundLayer;
    public float rotationSpeed = 15f;

    private Rigidbody rb;
    private Collider col;
    private Animator anim;
    private Vector2 moveInput;
    private RisingWater waterScript;
    private bool isGrounded;
    private float distToGround;
    private bool isJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        anim = GetComponentInChildren<Animator>();
        distToGround = col.bounds.extents.y;

        waterScript = FindAnyObjectByType<RisingWater>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        bool waterAllowsMovement = (waterScript == null || waterScript.canRise);

        if (value.isPressed && isGrounded && !isJumping && waterAllowsMovement)
        {
            isJumping = true;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            if (anim != null)
                anim.SetBool("IsJumping", true);
        }
    }

    void Update()
    {
        if (isJumping && isGrounded && rb.linearVelocity.y <= 0)
        {
            isJumping = false;
        }

        bool waterAllowsMovement = (waterScript == null || waterScript.canRise);

        if (anim != null)
        {
            anim.SetBool("IsJumping", !isGrounded);
            anim.SetBool("IsGrounded", isGrounded);

            bool isEffectivelyWalking = Mathf.Abs(moveInput.x) > 0.1f && waterAllowsMovement;
            anim.SetBool("IsWalking", isEffectivelyWalking);
        }

        if (Mathf.Abs(moveInput.x) > 0.1f && waterAllowsMovement)
        {
            Vector3 targetDirection = new Vector3(moveInput.x, 0f, 0f);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics.Raycast(col.bounds.center, Vector3.down, distToGround + 0.1f, groundLayer);

        float allowedHorizontalInput = (waterScript != null && !waterScript.canRise) ? 0 : moveInput.x;

        Vector3 extraGravity = Physics.gravity * (gravityMultiplier - 1f);
        rb.AddForce(extraGravity, ForceMode.Acceleration);

        bool wallRight = Physics.Raycast(col.bounds.center, Vector3.right, col.bounds.extents.x + 0.1f, groundLayer);
        bool wallLeft = Physics.Raycast(col.bounds.center, Vector3.left, col.bounds.extents.x + 0.1f, groundLayer);

        float finalMove = allowedHorizontalInput;
        if (wallRight && allowedHorizontalInput > 0) finalMove = 0;
        if (wallLeft && allowedHorizontalInput < 0) finalMove = 0;

        rb.linearVelocity = new Vector3(finalMove * moveSpeed, rb.linearVelocity.y, 0f);
    }
}