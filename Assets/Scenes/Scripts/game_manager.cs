using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Elements")]
    public TMP_Text scoreText;
    public GameObject gameOverPanel; 

    public int Score { get; private set; }

    private void Start()
    {
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void OnEnable()
    {
        Asteroid.OnTinyAsteroidDestroyed += AddScore;
    }

    private void OnDisable()
    {
        Asteroid.OnTinyAsteroidDestroyed -= AddScore;
    }

    private void AddScore(int points)
    {
        Score += points;
        if (scoreText != null)
        {
            scoreText.text = $"Score: {Score}";
        }
    }

    public void GameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Unpause
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); 
    }
}