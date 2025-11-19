using UnityEngine;

public class HideZone : MonoBehaviour
{
    [SerializeField] private GameObject hidePromptUI;

    private PlayerMovement player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerMovement>();
            TryShowPrompt();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hidePromptUI.SetActive(false);
            player = null;
        }
    }

    private void Update()
    {
        if (player == null) return;
        TryShowPrompt();
    }

    private void TryShowPrompt()
    {
        if (player.IsHiding)
            hidePromptUI.SetActive(false);
        else
            hidePromptUI.SetActive(true);
    }
}
