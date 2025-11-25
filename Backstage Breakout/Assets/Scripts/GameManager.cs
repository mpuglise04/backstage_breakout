using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject gameOverScreen;

    [SerializeField] private TextMeshProUGUI loseHeaderText;
    [SerializeField] private TextMeshProUGUI winHeaderText;

    // Optional: your smaller descriptive message text
    [SerializeField] private TextMeshProUGUI gameOverText;

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

    public void GameOver(bool didWin)
    {
        if (isGameOver) return;
        isGameOver = true;

        // Toggle big headers
        loseHeaderText.gameObject.SetActive(!didWin);
        winHeaderText.gameObject.SetActive(didWin);

        // Optional message under the header
        if (gameOverText != null)
        {
            gameOverText.text = didWin ? "You escaped!" : "You've been caught!";
        }

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

