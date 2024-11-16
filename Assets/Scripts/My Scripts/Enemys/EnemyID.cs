using UnityEngine;

public class EnemyID : MonoBehaviour
{
    public int sum; // シーン内の全ての敵キャラの加算用 1,1,1,…

    private static int enemyCount = 0;
    public int enemyId;

    void Awake()
    {
        enemyId = enemyCount++;
    }
}
