using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float runSpeed = 5f;
    public float jumpForce = 10f;
    public float slideDuration = 0.6f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private CapsuleCollider2D col;
    private Animator animator;

    private bool isGrounded;
    private bool isSliding;

    float originalHeight;
    Vector2 originalOffset;
     
    private bool JumpPressed;
    private bool slidePressed;


  

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();

        originalHeight = col.size.y;
        originalOffset = col.offset;
    }

    void Update()
    {
        CheckGround();
        HandleJump();
        HandleSlide();
        UpdateAnimator();
      
    }
    void FixedUpdate()
    {
        AutoRun();
    }
    
    public void OnJumpButton()
    {
        JumpPressed = true;
    }
    public void OnSlideButton()
    {
        slidePressed = true;
    }

    void AutoRun()
    {
        if (!isSliding)
        {
            rb.linearVelocity = new Vector2(runSpeed, rb.linearVelocity.y);
        }
    }

    void HandleJump()
    {
        if ((Input.GetKeyDown(KeyCode.Space) ||JumpPressed) && isGrounded && !isSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
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
            0.2f,
            groundLayer
        );
    }

    void UpdateAnimator()
    {
        animator.SetFloat("Speed", runSpeed);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsSliding", isSliding);
        if(!isGrounded && rb.linearVelocity.y<0)
        {
            animator.speed = 0.5f;
        } else
        {
            animator.speed = 1f;
        }
    }

    System.Collections.IEnumerator Slide()
    {
        isSliding = true;

        col.size = new Vector2(col.size.x, originalHeight / 2f);
        col.offset = new Vector2(
            col.offset.x,
            originalOffset.y - originalHeight / 4f
        );

        yield return new WaitForSeconds(slideDuration);

        col.size = new Vector2(col.size.x, originalHeight);
        col.offset = originalOffset;

        isSliding = false;
    }
}