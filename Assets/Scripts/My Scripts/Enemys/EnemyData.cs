using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "EnemyData", menuName = "CreateEnemyData")]
public class EnemyData : ScriptableObject
{
    // ================================
    // �G�l�~�[�̎��
    // ================================
    // �������ɑ��₵�Ă���������OK
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

    // �G�̎��
    [SerializeField] private EnemyType enemyType;
    // �G�̖��O
    [SerializeField] private string enemyName;
    // �G�����Ƃ��A�C�e��
    [SerializeField] private ItemData.Type itemtype;
    // �A�C�e���̃h���b�v��
    [SerializeField] private int Droprate;

    // �G��HP
    [SerializeField] private int maxHp;
    // �G�̍U����
    [SerializeField] private int ATK;
    // �G�̖h���
    [SerializeField] private int DEF;

    // �G�̎�ގ擾
    public EnemyType GetEnemyType()
    {
        return enemyType;
    }

    // �G�̖��O�擾
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

    // �A�C�e���̃h���b�v���擾
    public int GetDroprate()
    {
        return Droprate;
    }
}
