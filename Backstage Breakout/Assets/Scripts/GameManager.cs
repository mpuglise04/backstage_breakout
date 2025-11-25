using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI gameOverText;

    [SerializeField] private GameObject winHeaderText;   // “YOU WIN!”
    [SerializeField] private GameObject loseHeaderText;  // “GAME OVER”

    [Header("Intro UI")]
    [SerializeField] private TextMeshProUGUI introText;

    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);

        // IMPORTANT: Ensure headers start hidden
        if (winHeaderText != null) winHeaderText.SetActive(false);
        if (loseHeaderText != null) loseHeaderText.SetActive(false);
    }

    private void Start()
    {
        if (introText != null)
        {
            introText.text = "Reach the venue before time runs out!";
            introText.gameObject.SetActive(true);
            StartCoroutine(HideIntroText());
        }
    }

    private IEnumerator HideIntroText()
    {
        yield return new WaitForSeconds(1.5f);
        introText.gameObject.SetActive(false);
    }

    public void GameOver(bool playerWon)
    {
        if (isGameOver) return;
        isGameOver = true;

        // Hide both first to prevent overlap
        if (winHeaderText != null) winHeaderText.SetActive(false);
        if (loseHeaderText != null) loseHeaderText.SetActive(false);

        // Show correct header
        if (playerWon && winHeaderText != null)
            winHeaderText.SetActive(true);
        else if (!playerWon && loseHeaderText != null)
            loseHeaderText.SetActive(true);

        // Subtitle text
        if (gameOverText != null)
            gameOverText.text = playerWon ? "You Escaped!" : "You've Been Caught!";

        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);

        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
        }
    }
}
