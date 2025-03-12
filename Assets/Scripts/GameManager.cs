using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private bool isPaused;
    public event Action<float> onTimeChanged; // Evento para actualizar el UI del tiempo
    public event Action<int> onLivesChanged;  // Evento para actualizar el UI de vidas
    public event Action<bool> onGameOver;     // Evento para manejar el Game Over

    [Header("Game Settings")]
    public int startLives = 3;
    public float startTime = 120f; // Tiempo inicial de cuenta regresiva

    private int currentLives;
    private float gameTime;
    public bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            Debug.Log("Game manager se intancio");
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        StartLevel(); // Solo iniciar nivel si no estamos en el menú
    }

    void Update()
    {
        if (!isGameOver)
        {
            gameTime -= Time.deltaTime;
            if (gameTime <= 0)
            {
                gameTime = 0;
                GameOver(false); // Derrota por tiempo agotado
            }
            onTimeChanged?.Invoke(gameTime); // Notificar cambio de tiempo
        }
    }

    public void StartLevel()
    {
        currentLives = startLives;
        gameTime = startTime;
        isGameOver = false;
        Time.timeScale = 1;

        onTimeChanged?.Invoke(gameTime);
        onLivesChanged?.Invoke(currentLives);
        //AudioManager.Instance?.PlayMusic(AudioManager.Instance.Music); // Reproducir música del juego
    }

        //**  Disminuir Vidas
        public void DecreaseLives(int amount = 1)
    {
        if (isGameOver) return;

        currentLives -= amount;
        onLivesChanged?.Invoke(currentLives);

        if (currentLives <= 0)
        {
            GameOver(false); // Derrota por falta de vidas
        }
    }

    //**  Game Over
    void GameOver(bool hasWon)
    {
        isGameOver = true;
        onGameOver?.Invoke(hasWon);
        Debug.Log("Game Over");
    }

    // nameScene: Nombre de la escena
    public void LoadSceneByName(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Carga la siguiente escena por index
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Verifica si hay una siguiente escena
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No hay m�s escenas. �Has completado el juego!");
            ReloadCurrentScene(); // Recargar la primera escena
        }
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }
    public void ResetGame()
    {
        gameTime = startTime;  // Reiniciar el tiempo con la variable correcta
        currentLives = startLives;  // Reiniciar las vidas correctamente
        isGameOver = false;  // Asegurar que el juego no esté en estado de Game Over

        onTimeChanged?.Invoke(gameTime);
        onLivesChanged?.Invoke(currentLives);
    }
    public void OpenOptionsMenu()
    {
        SceneManager.LoadScene("OptionMenu", LoadSceneMode.Additive);
    }

    public void OpenCreditsMenu()
    {
        SceneManager.LoadScene("CreditsMenu", LoadSceneMode.Additive);
    }
    public float GetRemainingTime()
    {
        return gameTime;
    }

    public int GetCurrentLives()
    {
        return currentLives;

    }
    
}
/*Forma de utilizar funciones en otros scripts, llamar escenas por nombres
GameManager.instance.LoadSceneByName("Menu") */