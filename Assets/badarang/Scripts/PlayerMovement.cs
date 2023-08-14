using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveInput;
    public float maxSpeed;
    public float jumpSpeed;
    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask groundLayer;
    private bool isJumping;
    private float jumpTimeCounter;
    public float jumpTime;
    private bool isFacingRight = true;

    public bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = .2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = .3f;
    private Vector2 wallJumpingSpeed = new Vector2(8f, 16f);
    Rigidbody2D rb;
    public Transform wallCheck;
    public LayerMask wallLayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, groundLayer);
        
        if (isGrounded && Input.GetKeyDown(KeyCode.UpArrow))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpSpeed;
        }

        if (Input.GetKey(KeyCode.UpArrow) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpSpeed;
                jumpTimeCounter -= Time.deltaTime * 2;
            }
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            isJumping = false;
        }

        WallSlide();
        WallJump();
        if (!isWallJumping) Flip();
    }


    void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(moveInput * maxSpeed, rb.velocity.y);
        }
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, .2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !isGrounded && moveInput != 0f)
        {
            isWallSliding = true;
            Debug.Log(Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingSpeed.x, wallJumpingSpeed.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    
    private void Flip()
    {
        if (isFacingRight && moveInput < 0f || !isFacingRight && moveInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}