using UnityEngine;

public class Quest_Level_1 : MonoBehaviour
{
    public bool isClear = false;

    // �V�[�����̓G�L�����̐�
    private int totalEnemies;

    private int[] defeatedEnemies;

    bool fadestart = false;

    void Start()
    {
        // �V�[�����̑S�Ă̓G�L�������J�E���g
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

        // �S�Ă̓G���|���ꂽ���m�F
        if (totalEnemies <= defeatedEnemies[(int)type])
        {
            isClear = true;
            Debug.Log("Level Clear!");
        }
    }

    // �G�L���������v���郁�\�b�h
    public int SumEnemys()
    {
        int totalValue = 0;

        // �V�[�����̑S�Ă�EnemyID�R���|�[�l���g�����I�u�W�F�N�g������
        EnemyID[] enemyIDs = FindObjectsOfType<EnemyID>();

        // �eEnemyID��ID�����v
        foreach (var enemyID in enemyIDs)
        {
            totalValue += enemyID.sum;
        }

        return totalValue;
    }
}
