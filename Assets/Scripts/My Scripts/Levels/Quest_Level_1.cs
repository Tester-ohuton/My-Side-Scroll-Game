using UnityEngine;
using System.Collections.Generic;

public class Quest_Level_1 : MonoBehaviour
{
    private Dictionary<EnemyData.EnemyType, int> enemyCount;

    void Start()
    {
        enemyCount = new Dictionary<EnemyData.EnemyType, int>();

        // Initialize enemy count dictionary with all enemy types
        foreach (EnemyData.EnemyType type in System.Enum.GetValues(typeof(EnemyData.EnemyType)))
        {
            if (type != EnemyData.EnemyType.MAX_ENEMY)
            {
                enemyCount[type] = 0;
            }
        }
    }

    public void EnemyEncountered(EnemyData.EnemyType type)
    {
        if (enemyCount.ContainsKey(type))
        {
            enemyCount[type]++;
            Debug.Log(enemyCount[type]);
        }
    }

    public int GetEnemyCount(EnemyData.EnemyType type)
    {
        return enemyCount.ContainsKey(type) ? enemyCount[type] : 0;
    }
}
