using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : MonoBehaviour
{
    // �X�V���Ă������ǂ����i�G�L�����N�^�[�����Z
    public static bool IsUpdate = false;

    // �G�L�������i�[�z��(�G�L�����N�^�[�f�[�^����G�L�����N�^�[�̎�ސ����擾)
    public static int[] EnemyWarehouse = new int[(int)EnemyData.EnemyType.MAX_ENEMY];

    // �q�ɂ̒��g���������ς݂��ǂ���
    // �Q�[���J�n���P��̂�
    public static bool IsInit = false;

    public static void InitWarehouse()
    {
        // �������ς݂Ȃ��΂�
        if (IsInit) return;

        // �z��̒��g���O�ŏ�����
        for (int i = 0; i < EnemyWarehouse.Length; ++i)
        {
            EnemyWarehouse[i] = 0;
        }
        IsInit = true;
    }


    // �G�L�������i�[�z����擾
    public static int[] GetEnemyWarehouse()
    {
        return EnemyWarehouse;
    }

    // �Q�[���Ŏ擾�����G�L���������Z
    // @param...item �Q�[���V�[���œG�L�������i�[���Ă���z��
    public static void AddEnemyNum(int[] item)
    {
        // �z�񐔕�
        for (int i = 0; i < EnemyWarehouse.Length; ++i)
        {
            // �擾�����G�L������ۑ�
            EnemyWarehouse[i] += item[i];
        }
    }
}
