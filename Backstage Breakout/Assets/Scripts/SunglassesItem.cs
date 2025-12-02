using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SunglassesItem : MonoBehaviour
{
    public float invincibilityDuration = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.ActivateInvincibility(invincibilityDuration);
            }

            // Hide the collectible
            gameObject.SetActive(false);
        }
    }
}
