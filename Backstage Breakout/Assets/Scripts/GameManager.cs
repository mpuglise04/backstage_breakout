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
    [SerializeField] private TextMeshProUGUI gameOverTitle;

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

        // Title styling
        if (gameOverTitle != null)
        {
            if (playerWon)
            {
                gameOverTitle.text = "YOU WIN!";
                gameOverTitle.color = Color.green;
            }
            else
            {
                gameOverTitle.text = "GAME OVER";
                gameOverTitle.color = Color.red;
            }
        }

        // Description text
        if (gameOverText != null)
        {
            gameOverText.text = playerWon ? "You Escaped!" : "You've Been Caught!";
        }

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
