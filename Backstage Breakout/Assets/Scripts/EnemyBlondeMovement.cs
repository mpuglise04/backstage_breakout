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
            // Stop the enemy if not moving
            rb.velocity = new Vector2(0f, rb.velocity.y);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("Player"))
            GameManager.Instance.GameOver(false);
    }
}
