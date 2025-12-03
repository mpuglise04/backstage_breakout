using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackItem : MonoBehaviour
{
    [SerializeField] private float jetpackDuration = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement pm = other.GetComponent<PlayerMovement>();
            if (pm != null)
            {
                pm.ActivateJetpack(jetpackDuration);
            }

            // Hide the jetpack item so it cannot be collected again
            gameObject.SetActive(false);
        }
    }
}

