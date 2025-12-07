using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackItem : MonoBehaviour
{
    [SerializeField] private float jetpackDuration = 6f;

    [SerializeField] private JetpackMessageUI messageUI;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip collectSound;   // Assign your MP3 in Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement pm = other.GetComponent<PlayerMovement>();
            if (pm != null)
            {
                pm.ActivateJetpack(jetpackDuration);
            }

            // Show on-screen message
            if (messageUI != null)
            {
                messageUI.ShowMessage();
            }

            // Play collection sound in 2D
            if (collectSound != null)
            {
                Play2DSound(collectSound);
            }

            // Hide jetpack object
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
