using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
public static GameManager Instance {  get; private set; }
    private bool isPaused;

    [Header("Game Settings")]
    public int startLives = 3;

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
        // Inicialización
        currentLives = startLives;
        gameTime = 0f;
        isGameOver = false;
    }
    
    void Update()
    {
        if (!isGameOver)
        {
            // Actualizar tiempo
            gameTime += Time.deltaTime;
        }
    }
    
    //**  Disminuir Vidas
    public void DecreaseLives(int amount = 1)
    {
        if (isGameOver) return; 

        currentLives -= amount;

        if (currentLives <= 0)
        {
            GameOver();
        }
    }
    
    //**  Game Over
    void GameOver()
    {
        isGameOver = true;
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
    public void OpenOptionsMenu()
    {
        SceneManager.LoadScene("OptionMenu", LoadSceneMode.Additive);
    }

    public void OpenCreditsMenu()
    {
        SceneManager.LoadScene("CreditsMenu", LoadSceneMode.Additive);
    }
}
/*Forma de utilizar funciones en otros scripts, llamar escenas por nombres
GameManager.instance.LoadSceneByName("Menu") */