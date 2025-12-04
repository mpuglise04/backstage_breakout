using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    [SerializeField] private SunglassesTimerUI sunglassesTimerUI;
    private bool isInvincible = false;

    [Header("Jetpack Settings")]
    [SerializeField] private GameObject jetpackVisual;
    [SerializeField] private float jetpackUpwardForce = 10f;
    [SerializeField] private float simulatedGravity = 9.81f;

    private bool jetpackActive = false;
    private bool isFlying = false;

    // Spring Settings
    [Header("Spring Settings")]
    [SerializeField] private float springJumpForce = 18f; // tweak in Inspector
    private bool onSpringTile = false;

    //UI References
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI springPromptTMP;

    private const int PLAYER_LAYER = 8;
    private const int ENEMY_LAYER = 9;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;

        // Make sure the prompt is hidden at start
        if (springPromptTMP != null)
            springPromptTMP.gameObject.SetActive(false);
    }

    private void Update()
    {
        HandleMovement();
        HandleJetpackInput();
        HandleSpringInput();
    }

    private void FixedUpdate()
    {
        if (IsHiding)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isFlying)
        {
            rb.velocity += Vector2.down * simulatedGravity * Time.fixedDeltaTime;
        }
    }

    private void HandleMovement()
    {
        float move = Input.GetAxisRaw("Horizontal");

        if (!IsHiding)
        {
            rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
        }

        // Flip sprite
        if (move > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (move < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }

        // Animator
        if (anim != null)
        {
            anim.SetBool("run", move != 0);
        }

        // Hiding with H key
        if (canHide && Input.GetKey("h"))
        {
            StartHiding();
        }
        else
        {
            StopHiding();
        }
    }

    // Spring Input

    private void HandleSpringInput()
    {
        // Player is overlapping a spring tile and chooses to press Space
        if (onSpringTile && Input.GetKeyDown(KeyCode.Space) && !IsHiding)
        {
            SpringJump();
        }
    }

    private void SpringJump()
    {
        // Optional: cancel current vertical velocity so the bounce is consistent
        rb.velocity = new Vector2(rb.velocity.x, 0f);

        rb.AddForce(Vector2.up * springJumpForce, ForceMode2D.Impulse);

        // Hide prompt once we use the spring
        if (springPromptTMP != null)
            springPromptTMP.gameObject.SetActive(false);

        onSpringTile = false;
    }

    public void SetCanHide(bool value)
    {
        canHide = value;

        if (!value)
        {
            StopHiding();
        }
    }

    private void StartHiding()
    {
        if (IsHiding) return;

        Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_LAYER, true);

        if (rend != null)
            rend.sortingOrder = 0;

        IsHiding = true;

        // If we hide, also hide spring prompt to avoid confusion
        if (springPromptTMP != null)
            springPromptTMP.gameObject.SetActive(false);
    }

    private void StopHiding()
    {
        if (!IsHiding) return;

        Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_LAYER, false);

        if (rend != null)
            rend.sortingOrder = 2;

        IsHiding = false;
        isFlying = false;
    }

    // ───────── Invincibility ─────────

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

        if (sunglassesVisual != null) sunglassesVisual.SetActive(true);

        // Ignore collisions with enemies
        Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_LAYER, true);

        // Start UI countdown
        if (sunglassesTimerUI != null)
            sunglassesTimerUI.StartTimer(duration);

        yield return new WaitForSeconds(duration);

        isInvincible = false;

        if (sunglassesVisual != null) sunglassesVisual.SetActive(false);

        // Re-enable collisions
        Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_LAYER, false);
    }

    // ───────── Jetpack ─────────

    private void HandleJetpackInput()
    {
        // Only allow launch while jetpack buff is active and not already flying
        if (jetpackActive && Input.GetKeyDown(KeyCode.Space) && !isFlying && !IsHiding)
        {
            LaunchJetpack();
        }
    }

    private void LaunchJetpack()
    {
        isFlying = true;

        // Reset vertical velocity before impulse
        rb.velocity = new Vector2(rb.velocity.x, 0f);

        rb.AddForce(Vector2.up * jetpackUpwardForce, ForceMode2D.Impulse);
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

        // After buff ends, we can still be falling normally
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spring"))
        {
            onSpringTile = true;

            // Show "Press SPACE to bounce" prompt
            if (springPromptTMP != null && !IsHiding)
                springPromptTMP.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Spring"))
        {
            onSpringTile = false;

            // Hide prompt when we leave the spring
            if (springPromptTMP != null)
                springPromptTMP.gameObject.SetActive(false);
        }
    }
}
