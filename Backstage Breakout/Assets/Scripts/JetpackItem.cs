using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackItem : MonoBehaviour
{
    [SerializeField] private float jetpackDuration = 6f;

    [SerializeField] private JetpackMessageUI messageUI;

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

            // Hide jetpack object
            gameObject.SetActive(false);
        }
    }
}
