using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrunette : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("Horizontal Movement Bounds")]
    [SerializeField] private float leftX;
    [SerializeField] private float rightX;

    [Header("Movement Toggle")]
    [SerializeField] private bool shouldMove = false; // Set per instance
    private Animator animator;
    private bool movingRight = true;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.SetBool("moving", shouldMove);
    }

    private void Update()
    {
        if (shouldMove)
        {
            MoveEnemy();
        }
        else
        {
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


