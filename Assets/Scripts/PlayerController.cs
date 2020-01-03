#pragma warning disable 0649
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float lowJumpMultiplier;
    [SerializeField] private float fallMultiplier;

    [SerializeField] private float groundedSkin = 0.05f;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private UnityEvent OnJumpEvent = new UnityEvent();
    [SerializeField] private UnityEvent OnLandEvent = new UnityEvent();
    
    private bool jumpRequest;
    private Vector2 playerSize, boxSize;
    private Rigidbody2D rb;

    private bool wasGrounded;
    private bool IsGrounded
    {
        get
        {
            Vector2 boxCenter = (Vector2) transform.position + (playerSize.y + boxSize.y) * 0.5f * Vector2.down;
            return Physics2D.OverlapBoxNonAlloc(boxCenter, boxSize, 0f, new Collider2D[1], whatIsGround) > 0;
        }
    }
    
    private void Awake()
    {
        playerSize = GetComponent<BoxCollider2D>().size;
        boxSize = new Vector2(playerSize.x, groundedSkin);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded)
            jumpRequest = true;
    }

    private void FixedUpdate()
    {
        if (jumpRequest)
        {
            rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);

            OnJumpEvent.Invoke();
            jumpRequest = false;
            wasGrounded = false;
        }
        else
        {
            float velocityY = rb.velocity.y;
            rb.gravityScale = velocityY < 0f ? fallMultiplier :
                velocityY > 0f && !Input.GetButton("Jump") ? lowJumpMultiplier : 1f;
            
            if (!wasGrounded && IsGrounded)
            {
                OnLandEvent.Invoke();
                wasGrounded = true;
            }
        }
    }
}
