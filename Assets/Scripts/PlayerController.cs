#pragma warning disable 0649
using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float lowJumpMultiplier;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float forceFallMultiplier;

    [SerializeField] private float groundedSkin = 0.05f;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private UnityEvent OnJumpEvent = new UnityEvent();
    [SerializeField] private UnityEvent OnLandEvent = new UnityEvent();
    
    [SerializeField] private UnityEvent OnCantSwapGravityEvent = new UnityEvent();
    [SerializeField] private UnityEvent OnCanSwapGravityEvent = new UnityEvent();

    private float startX;
    
    private bool jumpRequest, canSwapGravity = true;
    private Vector2 playerSize, boxSize;
    private Rigidbody2D rb;

    private bool wasGrounded;
    private bool IsGrounded
    {
        get
        {
            Vector2 boxCenter = (Vector2) transform.position + (playerSize.y + boxSize.y) * 0.5f * (Physics2D.gravity.y < 0f ? Vector2.down : Vector2.up);
            return Physics2D.OverlapBoxNonAlloc(boxCenter, boxSize, 0f, new Collider2D[1], whatIsGround) > 0;
        }
    }
    
    private void Awake()
    {
        playerSize = GetComponent<BoxCollider2D>().size;
        boxSize = new Vector2(playerSize.x, groundedSkin);
        rb = GetComponent<Rigidbody2D>();

        startX = transform.position.x;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") || 
            Physics2D.gravity.y < 0f && Input.GetKeyDown(KeyCode.W) || 
            Physics2D.gravity.y > 0f && Input.GetKeyDown(KeyCode.S))
        {
            if (IsGrounded)
                jumpRequest = true;
        }

        if (canSwapGravity && Input.GetButtonDown("Fire1"))
        {
            OnCantSwapGravityEvent.Invoke();
            canSwapGravity = false;
            
            Physics2D.gravity = -Physics2D.gravity;
            transform.rotation = Quaternion.Euler(0f, 0f, Physics2D.gravity.y < 0f ? 0f : 180f);
        }
    }

    private void FixedUpdate()
    {
        if (jumpRequest)
        {
            rb.AddForce((Physics2D.gravity.y < 0f ? Vector2.up : Vector2.down) * jumpVelocity, ForceMode2D.Impulse);

            OnJumpEvent.Invoke();
            jumpRequest = false;
            wasGrounded = false;
        }
        else
        {
            float velocityY = rb.velocity.y;
            if (Physics2D.gravity.y > 0f) velocityY = -velocityY;
            
            if (velocityY < 0f)
            {
                rb.gravityScale = fallMultiplier;
                if (Physics2D.gravity.y > 0f && Input.GetKey(KeyCode.W) || Physics2D.gravity.y < 0f && Input.GetKey(KeyCode.S))
                    rb.gravityScale *= forceFallMultiplier;
            }
            else if (Physics2D.gravity.y > 0f && Input.GetKey(KeyCode.W) || Physics2D.gravity.y < 0f && Input.GetKey(KeyCode.S))
                rb.gravityScale *= forceFallMultiplier;
            else if (velocityY > 0f && !(Input.GetButton("Jump") || Physics2D.gravity.y < 0f && Input.GetKey(KeyCode.W) || Physics2D.gravity.y > 0f && Input.GetKey(KeyCode.S)))
                rb.gravityScale = lowJumpMultiplier;
            else
                rb.gravityScale = 1f;

            if (IsGrounded)
            {
                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                    wasGrounded = true;
                    
                    if (!canSwapGravity)
                    {
                        OnCanSwapGravityEvent.Invoke();
                        canSwapGravity = true;
                    }
                }
            }
            else
            {
                wasGrounded = false;
            }
        }

        if (transform.position.x < startX)
        {
            transform.position += new Vector3(Time.fixedDeltaTime, 0f);
            if (transform.position.x > startX) transform.position = new Vector3(startX, transform.position.y);
        }
    }
}
