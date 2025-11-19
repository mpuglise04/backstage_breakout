using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject gameOverScreen;
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

    public void GameOver(string message = "Game Over")
    {
        if (isGameOver) return;

        isGameOver = true;

        if (gameOverText != null)
            gameOverText.text = message;

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
