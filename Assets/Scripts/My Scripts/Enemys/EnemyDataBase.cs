using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataBase", menuName = "CreateEnemyDataBase")]
public class EnemyDataBase : ScriptableObject
{
    // ================================
    // �G�l�~�[�f�[�^���܂Ƃ߂�
    // ================================

    [SerializeField]
    private List<EnemyData> enemyLists = new List<EnemyData>();

    // �A�C�e�����X�g��Ԃ�
    public List<EnemyData> GetEnemyLists()
    {
        return enemyLists;
    }
}