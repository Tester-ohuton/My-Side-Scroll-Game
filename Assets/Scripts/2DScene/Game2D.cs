using UnityEngine;
using UnityEngine.SceneManagement;

public class Game2D : MonoBehaviour
{
    public static Game2D Instance;

    public event System.Action OnGameOver;
    public event System.Action OnGameClear;

    public int totalEnemies;
    public int enemiesDefeated;
    public bool isGameClear = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnemyDefeated()
    {
        enemiesDefeated++;
        UIManager.Instance.UpdateEnemyCount(enemiesDefeated);

        if (enemiesDefeated >= totalEnemies)
        {
            GameClear();
        }
    }

    public void GameOver()
    {
        OnGameOver?.Invoke();
        Debug.Log("Game Over!");
    }

    private void GameClear()
    {
        isGameClear = true;
        OnGameClear?.Invoke();
        Debug.Log("Game Clear!");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
