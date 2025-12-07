using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SunglassesItem : MonoBehaviour
{
    [Header("Invincibility Settings")]
    public float invincibilityDuration = 10f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip collectSound;   // Assign your MP3 in Inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.ActivateInvincibility(invincibilityDuration);
            }

            // Play collection sound in 2D
            if (collectSound != null)
            {
                Play2DSound(collectSound);
            }

            // Hide the collectible
            gameObject.SetActive(false);
        }
    }

    private void Play2DSound(AudioClip clip)
    {
        GameObject tempGO = new GameObject("TempAudio");
        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.spatialBlend = 0f; // fully 2D
        aSource.Play();
        Destroy(tempGO, clip.length);
    }
}


