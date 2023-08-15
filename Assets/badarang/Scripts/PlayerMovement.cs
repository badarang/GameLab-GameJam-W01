using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveInput;
    public float maxSpeed;
    public float jumpSpeed;
    public bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask groundLayer;
    public bool isJumping;
    public float jumpTimeCounter;
    public float jumpTime;
    private bool isFacingRight = true;
    public bool isCoyoteTime;
    private float dirtCreateDelay = 0f;

    public bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingCounter;
    private float wallJumpingDuration = .05f;
    private Vector2 wallJumpingSpeed = new Vector2(16f, 16f);
    Rigidbody2D rb;
    public Transform wallCheck;
    public LayerMask wallLayer;
    public Animator squashStretchAnimator;
    
    [SerializeField] private GameObject testParticleSystem = default;
    [SerializeField] private GameObject dirtParticle = default;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        if (moveInput != 0) gameObject.transform.SetParent(null);
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, groundLayer);
        if (dirtCreateDelay > 0f)
        {
            dirtCreateDelay -= .01f;
        }
        
        if (isGrounded && moveInput != 0 && dirtCreateDelay <= 0 && Mathf.Abs(rb.velocity.x) > 1.1f)
        {
            dirtCreateDelay = 1.5f;
            GameObject particle = Instantiate(dirtParticle, rb.transform.position + new Vector3(isFacingRight ? -.5f : .5f, -.5f, 0), transform.rotation);
            ParticleSystem particlesys = particle.GetComponent<ParticleSystem>();
            particlesys.Play();
        }
        
        if ((isGrounded || isCoyoteTime) && Input.GetKeyDown(KeyCode.UpArrow))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpSpeed;
            squashStretchAnimator.SetTrigger("Jump");
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

    #region WallJump
    
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, .2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !isGrounded && moveInput != 0f)
        {
            isWallSliding = true;
            //Debug.Log(Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }

        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
    }

    private void WallJump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && isWallSliding)
        {
            isWallJumping = true;
            squashStretchAnimator.SetTrigger("Jump");
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }

        if (isWallJumping)
        {
            rb.velocity = new Vector2(-moveInput * wallJumpingSpeed.x, wallJumpingSpeed.y);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
   
    #endregion
    
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && rb.velocity.y <= 0)
        {
            squashStretchAnimator.SetTrigger("Landing");
        }
    }


    #region CoyoteTime
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && rb.velocity.y <= 0)
        {
            isCoyoteTime = true;
            StartCoroutine(Co_CoyoteTimer());
        }
    }

    IEnumerator Co_CoyoteTimer()
    {
        yield return new WaitForSecondsRealtime(.2f);
        isCoyoteTime = false;
    }
    
    #endregion
    
    
    #region Die

    void HandlingDie()
    {
        GameObject particle = Instantiate(testParticleSystem);
        ParticleSystem particlesys = particle.GetComponent<ParticleSystem>();
        particlesys.Play();
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spike"))
        {
            HandlingDie();
        }
    }

    #endregion
    
}