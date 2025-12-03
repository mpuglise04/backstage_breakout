using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlondeMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("Horizontal Movement Bounds")]
    [SerializeField] private float leftX;
    [SerializeField] private float rightX;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private bool shouldMove = false; // Set this in Inspector for each instance
    private Animator animator;
    private bool movingRight = true;

    // Track the player if they are inside the trigger
    private PlayerMovement playerInside = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.SetBool("moving", shouldMove);
    }

    private void Update()
    {
        // Only move if the Animator's "moving" parameter is true
        if (animator.GetBool("moving"))
        {
            MoveEnemy();
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        // If player is inside trigger, check every frame in case invincibility ended
        if (playerInside != null)
        {
            TryDamagePlayer();
        }
    }

    private void MoveEnemy()
    {
        if (movingRight)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            spriteRenderer.flipX = false;

            if (transform.position.x >= rightX)
                movingRight = false;
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            spriteRenderer.flipX = true;

            if (transform.position.x <= leftX)
                movingRight = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerInside = collision.collider.GetComponent<PlayerMovement>();
            TryDamagePlayer();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            playerInside = null;
    }

    private void TryDamagePlayer()
    {
        if (playerInside != null && !playerInside.IsInvincible())
        {
            GameManager.Instance.GameOver(false);
            playerInside = null; // Prevent multiple triggers
        }
    }
}