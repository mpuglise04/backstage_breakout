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
    [SerializeField] private float jetpackUpwardForce = 10f;
    [SerializeField] private float simulatedGravity = 9.81f;

    private bool jetpackActive = false;
    private bool isFlying = false;

    private const int PLAYER_LAYER = 8;
    private const int ENEMY_LAYER = 9;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
    }

    private void Update()
    {
        HandleMovement();
        HandleJetpackInput();
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

        if (move > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (move < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }

        if (anim != null)
        {
            anim.SetBool("run", move != 0);
        }

        if (canHide && Input.GetKey("h"))
        {
            StartHiding();
        }
        else
        {
            StopHiding();
        }
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

        yield return new WaitForSeconds(duration);

        isInvincible = false;
        if (sunglassesVisual != null) sunglassesVisual.SetActive(false);
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
}
