using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticStatus : MonoBehaviour
{
    // =====================================
    // �V�[�����ׂ��X�e�[�^�X�Ǘ�
    // =====================================
    // �X�e�[�W�I�𕔉��̑����i�ɂ����
    // �X�e�[�^�X���ϓ�
    // �I�����X�e�[�^�X�ۑ����Q�[���Ŏg�p
    // =====================================

    // --- �z�肷��g���� ---
    // �I���V�[���ő����i�ɉ����ăX�e�[�^�X�v�Z
    // SetStatus�ŃX�e�[�^�X�ݒ�
    // �Q�[���V�[����Get�֐����g���ݒ�

    // �̗�
    public static int PlayerHP = 100;

    // �U����
    public static int PlayerATK = 10;

    // �h���
    public static int PlayerDEF = 10;

    // �^
    public static int PlayerLUCK = 10;

    // �̗͎擾
    public static int GetPlayerHP()
    {
        return PlayerHP;
    }

    // �U���͎擾
    public static int GetPlayerATK()
    {
        return PlayerATK;
    }

    // �h��͎擾
    public static int GetPlayerDEF()
    {
        return PlayerDEF;
    }

    // �^�擾
    public static int GetPlayerLUCK()
    {
        return PlayerLUCK;
    }

    // �I���V�[���ő����i�ɉ����ăX�e�[�^�X���Z�p
    public static void SetStatus(int hp, int atk, int def, int luck)
    {
        PlayerHP += hp;
        PlayerATK += atk;
        PlayerDEF += def;
        PlayerLUCK += luck;
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
