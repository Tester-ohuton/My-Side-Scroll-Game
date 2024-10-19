using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataBase", menuName = "CreateEnemyDataBase")]
public class EnemyDataBase : ScriptableObject
{
    // ================================
    // エネミーデータをまとめる
    // ================================

    [SerializeField]
    private List<EnemyData> enemyLists = new List<EnemyData>();

    // アイテムリストを返す
    public List<EnemyData> GetEnemyLists()
    {
        return enemyLists;
    }
}
