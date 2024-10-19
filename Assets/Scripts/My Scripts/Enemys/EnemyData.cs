using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "EnemyData", menuName = "CreateEnemyData")]
public class EnemyData : ScriptableObject
{
    // ================================
    // エネミーの種類
    // ================================
    // ↓ここに増やしていくだけでOK
    // ================================
    public enum EnemyType
    {
        ENEMY_1,
        ENEMY_2,
        ENEMY_3,
        ENEMY_4,
        ENEMY_5,
        ENEMY_6,


        MAX_ENEMY
    }

    // 敵の種類
    [SerializeField] private EnemyType enemyType;
    // 敵の名前
    [SerializeField] private string enemyName;
    // 敵が落とすアイテム
    [SerializeField] private ItemData.Type itemtype;
    // アイテムのドロップ率
    [SerializeField] private int Droprate;

    // 敵のHP
    [SerializeField] private int maxHp;
    // 敵の攻撃力
    [SerializeField] private int ATK;
    // 敵の防御力
    [SerializeField] private int DEF;

    // 敵の種類取得
    public EnemyType GetEnemyType()
    {
        return enemyType;
    }

    // 敵の名前取得
    public string GetEnemyName()
    {
        return enemyName;
    }

    public int GetHp()
    {
        return maxHp;
    }

    public int GetATK()
    {
        return ATK;
    }

    public int GetDEF()
    {
        return DEF;
    }

    // アイテムのドロップ率取得
    public int GetDroprate()
    {
        return Droprate;
    }
}
