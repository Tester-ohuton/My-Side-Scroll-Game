using UnityEngine;

public class Quest_Level_1 : MonoBehaviour
{
    public bool isClear = false;

    // シーン内の敵キャラの数
    private int totalEnemies;

    private int[] defeatedEnemies;

    bool fadestart = false;

    void Start()
    {
        // シーン内の全ての敵キャラをカウント
        totalEnemies = SumEnemys();
        defeatedEnemies = new int[(int)EnemyData.EnemyType.MAX_ENEMY];
        Debug.Log($"totalEnemies: {totalEnemies}");
    }

    private void Update()
    {
        if (isClear)
        {
            if (!fadestart)
            {
                fadestart = true;
                FadeManager.Instance.LoadScene("Result", 2.0f);
            }
        }
    }

    public void EnemyDefeated(EnemyData.EnemyType type)
    {
        defeatedEnemies[(int)type]++;
        Debug.Log($"defeatedEnemies:{defeatedEnemies[(int)type]}");

        // 全ての敵が倒されたか確認
        if (totalEnemies <= defeatedEnemies[(int)type])
        {
            isClear = true;
            Debug.Log("Level Clear!");
        }
    }

    // 敵キャラを合計するメソッド
    public int SumEnemys()
    {
        int totalValue = 0;

        // シーン内の全てのEnemyIDコンポーネントを持つオブジェクトを検索
        EnemyID[] enemyIDs = FindObjectsOfType<EnemyID>();

        // 各EnemyIDのIDを合計
        foreach (var enemyID in enemyIDs)
        {
            totalValue += enemyID.sum;
        }

        return totalValue;
    }
}
