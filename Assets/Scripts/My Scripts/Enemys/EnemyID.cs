using UnityEngine;

public class EnemyID : MonoBehaviour
{
    public int sum; // �V�[�����̑S�Ă̓G�L�����̉��Z�p 1,1,1,�c

    private static int enemyCount = 0;
    public int enemyId;

    void Awake()
    {
        enemyId = enemyCount++;
    }
}
