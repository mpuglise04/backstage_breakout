using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer rend;
    private bool grounded;
    private Vector3 originalScale;

    private bool canHide = false;
    public bool IsHiding { get; private set; } = false;

    [Header("Invincibility Settings")]
    [SerializeField] private GameObject sunglassesVisual; // Child object for visual feedback
    private bool isInvincible = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Connects to Rigidbody2D
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        // Flip while keeping original size
        if (move > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (move < 0)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

        anim.SetBool("run", move != 0);

        // Hiding logic: hold H while in a trashcan trigger
        if (canHide && Input.GetKey("h"))
        {
            StartHiding();
        }
        else
        {
            StopHiding();
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void FixedUpdate()
    {
        if (!IsHiding)
            rb.velocity = new Vector2(rb.velocity.x, 0);
        else
            rb.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if touching the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Left ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Can hide when inside these trashcans collider
        if (other.gameObject.name.Equals("Trashcan 1") || other.gameObject.name.Equals("Trashcan 2"))
        {
            canHide = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("Trashcan 1") || other.gameObject.name.Equals("Trashcan 2"))
        {
            canHide = false;
            StopHiding();
        }
    }

    public void StartHiding()
    {
        if (IsHiding) return;

        Physics2D.IgnoreLayerCollision(8, 9, true);

        rend.sortingOrder = 1;

        IsHiding = true;
    }

    public void StopHiding()
    {
        if (!IsHiding) return;

        Physics2D.IgnoreLayerCollision(8, 9, false);

        rend.sortingOrder = 2;

        IsHiding = false;
    }

    // Allows other scripts to check if the player is invincible
    public bool IsInvincible()
    {
        return isInvincible;
    }


    public void ActivateInvincibility(float duration)
    {
        if (!isInvincible)
            StartCoroutine(InvincibilityCoroutine(duration));
    }

    private IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;

        // Show sunglasses
        if (sunglassesVisual != null)
            sunglassesVisual.SetActive(true);

        // Wait for the duration
        yield return new WaitForSeconds(duration);

        // End invincibility
        isInvincible = false;

        // Hide sunglasses
        if (sunglassesVisual != null)
            sunglassesVisual.SetActive(false);
    }

}
