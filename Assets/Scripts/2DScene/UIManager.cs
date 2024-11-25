using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Text enemyCountText;
    public GameObject gameOverPanel;
    public GameObject gameClearPanel;

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

    public void UpdateEnemyCount(int count)
    {
        enemyCountText.text = "Enemies Defeated: " + count;
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void ShowGameClear()
    {
        gameClearPanel.SetActive(true);
    }
}
