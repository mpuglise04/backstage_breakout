using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer rend;
    private Vector3 originalScale;

    private bool canHide = false;
    public bool IsHiding { get; private set; } = false;

    [Header("Invincibility Settings")]
    [SerializeField] private GameObject sunglassesVisual;
    private bool isInvincible = false;

    [Header("Jetpack Settings")]
    [SerializeField] private GameObject jetpackVisual;
    [SerializeField] private float jetpackUpwardForce; // Adjust to taste
    [SerializeField] private float simulatedGravity;  // Downward force per second
    private bool jetpackActive = false;
    private bool isFlying = false; // Track if jetpack has launched

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    private void Update()
    {
        HandleMovement();
        HandleJetpackInput();
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

        // Hiding logic
        if (canHide && Input.GetKey("h"))
            StartHiding();
        else
            StopHiding();
    }

    private void HandleJetpackInput()
    {
        if (jetpackActive && Input.GetKeyDown(KeyCode.Space) && !isFlying)
        {
            LaunchJetpack();
        }

        // Simulate gravity if flying
        if (isFlying)
        {
            rb.AddForce(Vector2.down * simulatedGravity, ForceMode2D.Force);
        }
    }

    private void LaunchJetpack()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f); // Reset vertical velocity
        rb.AddForce(Vector2.up * jetpackUpwardForce, ForceMode2D.Impulse);
        isFlying = true;
    }

    private void FixedUpdate()
    {
        if (IsHiding)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Stop flying when player hits ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isFlying = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("Trashcan 1") || other.gameObject.name.Equals("Trashcan 2"))
            canHide = true;
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

    public bool IsInvincible() => isInvincible;

    public void ActivateInvincibility(float duration)
    {
        if (!isInvincible)
            StartCoroutine(InvincibilityCoroutine(duration));
    }

    private IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;
        if (sunglassesVisual != null) sunglassesVisual.SetActive(true);
        yield return new WaitForSeconds(duration);
        isInvincible = false;
        if (sunglassesVisual != null) sunglassesVisual.SetActive(false);
    }

    public void ActivateJetpack(float duration)
    {
        if (!jetpackActive)
            StartCoroutine(JetpackCoroutine(duration));
    }

    private IEnumerator JetpackCoroutine(float duration)
    {
        jetpackActive = true;
        if (jetpackVisual != null) jetpackVisual.SetActive(true);

        float timer = duration;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        jetpackActive = false;
        if (jetpackVisual != null) jetpackVisual.SetActive(false);
    }
}
