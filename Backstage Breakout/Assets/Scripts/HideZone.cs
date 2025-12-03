using UnityEngine;

public class HideZone : MonoBehaviour
{
    [SerializeField] private GameObject hidePromptUI;

    private PlayerMovement player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        player = other.GetComponent<PlayerMovement>();

        if (player != null)
        {
            player.SetCanHide(true);
            TryShowPrompt();
        }

        Debug.Log("Player entered HideZone");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (player != null)
        {
            player.SetCanHide(false);
        }

        if (hidePromptUI != null)
        {
            hidePromptUI.SetActive(false);
        }

        Debug.Log("Player exited HideZone");
        player = null;
    }

    private void Update()
    {
        if (player == null) return;
        TryShowPrompt();
    }

    private void TryShowPrompt()
    {
        if (hidePromptUI == null || player == null) return;

        if (player.IsHiding)
        {
            hidePromptUI.SetActive(false);
        }
        else
        {
            hidePromptUI.SetActive(true);
        }
    }
}
