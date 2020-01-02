#pragma warning disable 0649
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpVelocity = 5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private float fallMultiplier = 2.5f;

    [SerializeField] private float groundedSkin = 0.05f;
    [SerializeField] private LayerMask whatIsGround;
    
    private bool jumpRequest;
    private Vector2 playerSize, boxSize;
    private Rigidbody2D rb;

    private void Awake()
    {
        playerSize = GetComponent<BoxCollider2D>().size;
        boxSize = new Vector2(playerSize.x, groundedSkin);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
            jumpRequest = true;
    }

    private bool IsGrounded()
    {
        Vector2 boxCenter = (Vector2) transform.position + (playerSize.y + boxSize.y) * 0.5f * Vector2.down;
        return Physics2D.OverlapBox(boxCenter, boxSize, 0f, whatIsGround) != null;
    }

    private void FixedUpdate()
    {
        if (jumpRequest)
        {
            rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
            jumpRequest = false;
        }
        else
        {
            float velocityY = rb.velocity.y;
            rb.gravityScale = velocityY < 0f ? fallMultiplier :
                velocityY > 0f && !Input.GetButton("Jump") ? lowJumpMultiplier : 1f;
        }
    }
}
