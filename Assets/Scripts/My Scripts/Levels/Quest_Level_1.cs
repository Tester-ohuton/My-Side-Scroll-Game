using UnityEngine;
using UnityEngine.Events;

public class Quest_Level_1 : MonoBehaviour
{
    public int clearNum;

    public static UnityEvent OnEnemyDestroyCountEvent = new UnityEvent();

    public static UnityEvent OngameClearEvent = new UnityEvent();

    private int enemyCounter;
    private bool isGameClear;

    void Start()
    {
        ResetScore();

        OnEnemyDestroyCountEvent.AddListener(() =>
        {
            AddScore(1);
        });

        OngameClearEvent.AddListener(() =>
        {
            GameClear();
        });
    }

    private void Update()
    {
        if (!isGameClear)
        {
            if (clearNum <= enemyCounter)
            {
                OngameClearEvent.Invoke();
                isGameClear = true;
            }
        }
    }

    private void ResetScore()
    {
        enemyCounter = 0;
    }

    private void AddScore(int point)
    {
        enemyCounter += point;
    }

    public void GameClear()
    {
        FadeManager.Instance.LoadScene("Game Clear", 2.0f);   
    }
}