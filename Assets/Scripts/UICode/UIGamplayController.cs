using UnityEngine;
using UnityEngine.UI;
using TMPro; // Importa TextMeshPro
using UnityEngine.SceneManagement; // Para manejar escenas

public class UIGamplayController : MonoBehaviour
{
    public static UIGamplayController Instance;

    [Header("UI Panels")]
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject pausePanel; // Asigna un panel de pausa en el inspector

    [Header("UI Elements")]
    public TMP_Text timeText;
    public TMP_Text livesText;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.onTimeChanged += UpdateTimeUI;
            GameManager.Instance.onLivesChanged += UpdateLivesUI;
            GameManager.Instance.onGameOver += OnGameOver;
        }

        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        UpdateTimeUI(GameManager.Instance.GetRemainingTime());
        UpdateLivesUI(GameManager.Instance.GetCurrentLives());
    }
    public void clicSound()
    {
        AudioManager.Instance.PlayFX("ClicAlerta");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.onTimeChanged -= UpdateTimeUI;
            GameManager.Instance.onLivesChanged -= UpdateLivesUI;
            GameManager.Instance.onGameOver -= OnGameOver;
        }
    }

    private void UpdateTimeUI(float time)
    {
        if (timeText != null)
        {
            timeText.text = "TIME: " + FormatTime(time);
        }
    }

    // Formatea el tiempo en minutos y segundos
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void UpdateLivesUI(int lives)
    {
        if (livesText != null)
        {
            livesText.text = $"LIVES: {lives}";
        }
    }

    private void OnGameOver(bool hasWon)
    {
        if (hasWon && winPanel != null)
        {
            winPanel.SetActive(true);
        }
        else if (!hasWon && losePanel != null)
        {
            losePanel.SetActive(true);
        }

        Time.timeScale = 0;
    }
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1; // Pausa o reanuda el tiempo
        if (pausePanel != null)
        {
            pausePanel.SetActive(isPaused); // Muestra el panel si está pausado
        }
    }
    // Reiniciar el nivel actual
    public void RestartGame()
    {
        Time.timeScale = 1; // Asegurar que el tiempo se reinicie
        GameManager.Instance.ResetGame(); // Reiniciar variables
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // Cargar la escena en la posición 1 del Build Settings
    }

    // Reanudar el juego
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    // Volver al menú principal
    public void BackToMenu()
    {
        Time.timeScale = 1; // Asegurar que el tiempo se reinicie
        SceneManager.LoadScene("MainMenu"); // Asegúrate de que la escena del menú principal se llame "MainMenu"
    }
}
