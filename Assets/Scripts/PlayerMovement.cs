using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float runSpeed = 5f;

    [Header("Jump Setting ")]
    public float jumpForce = 10f;
    public float jumpCutMultiplier = 0.5f;   // Controls short hop
    public float coyoteTime = 0.15f;         // Jump forgiveness
    public float jumpBufferTime = 0.15f;

    [Header("Slide")]
    public float slideDuration = 0.6f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.3f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private CapsuleCollider2D col;
    private Animator animator;

    private bool isGrounded;
    private bool isSliding;
    bool hasControl = true;

    private float coyoteCounter;
    private float jumpBufferCounter;

    float originalHeight;
    Vector2 originalOffset;
     
    private bool JumpPressed;
    private bool slidePressed;

    [Header("Auto Slide Settings")]
    public bool isAutomated = false; // Flag to check if we are in a "cutscene" slide
    private Vector2 automatedVelocity;




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();

        originalHeight = col.size.y;
        originalOffset = col.offset;
        //rigidbody setting
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        if (isAutomated) return;
        if (!hasControl) return;
        CheckGround();
        HandleTimers();
        HandleJumpInput();
        HandleSlideInput();
        UpdateAnimator();
      
    }
    void FixedUpdate()
    {
        if (isAutomated)
        {
            // Force the player to move at the slide speed
            rb.linearVelocity = automatedVelocity;
            return; // Stop here! Don't run the normal movement code below.
        }
        if (!hasControl) return;

        HandleMovement();
    }

    void HandleMovement()
    {
        if (!isSliding)
        {
            rb.velocity = new Vector2(runSpeed, rb.velocity.y);
        }
    }

    void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }

        if (jumpBufferCounter > 0f && coyoteCounter > 0f)
        {
            Jump();
            jumpBufferCounter = 0f;
        }

        // Variable jump height (short hop)
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animator.SetTrigger("Jump");
    }

    void HandleTimers()
    {
        if (isGrounded)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.deltaTime;

        jumpBufferCounter -= Time.deltaTime;
    }

    //public void OnJumpButton()
    //{
    //    JumpPressed = true;
    //}
    void HandleSlideInput()
    {
        if (Input.GetKeyDown(KeyCode.S) && isGrounded && !isSliding)
        {
            StartCoroutine(Slide());
        }
    }

    System.Collections.IEnumerator Slide()
    {
        isSliding = true;

        col.size = new Vector2(col.size.x, originalHeight / 2f);
        col.offset = new Vector2(originalOffset.x, originalOffset.y - originalHeight / 4f);

        yield return new WaitForSeconds(slideDuration);

        col.size = new Vector2(col.size.x, originalHeight);
        col.offset = originalOffset;

        isSliding = false;
    }

    //public void OnSlideButton()
    //{
    //    slidePressed = true;
    //}

    //void AutoRun()
    //{
    //    if (!isSliding)
    //    {
    //        rb.linearVelocity = new Vector2(runSpeed, rb.linearVelocity.y);
    //    }
    //}
    public void SetControl(bool state)
    {
        hasControl = state;
        rb.velocity = Vector2.zero;
        rb.isKinematic = !state;
        col.enabled = state;
    }
    void HandleJump()
    {
        if ((Input.GetKeyDown(KeyCode.Space) ||JumpPressed) && isGrounded && !isSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump");
            JumpPressed = false;
        }
    }

    void HandleSlide()
    {
        if ((Input.GetKeyDown(KeyCode.S) || slidePressed) && isGrounded && !isSliding)
        {
            slidePressed = false;
            StartCoroutine(Slide());
        }
    }

  
    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer
        );
    }

    void UpdateAnimator()
    {
        animator.SetFloat("Speed", runSpeed);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsSliding", isSliding);
    }
    public void StartAutomatedSlide(Vector2 velocity)
    {
        isAutomated = true;
        automatedVelocity = velocity;

        // 1. Play the slide animation immediately
        if (animator != null) animator.Play("New Slide");

        // 2. Optional: Disable gravity if you want a perfectly straight slide
        // rb.gravityScale = 0; 
    }

    public void StopAutomatedSlide()
    {
        isAutomated = false;

        // Return to normal (Running or Idle)
        if (animator != null) animator.Play("New Running");

        // Reset gravity if you changed it
        // rb.gravityScale = 1;
    }

}