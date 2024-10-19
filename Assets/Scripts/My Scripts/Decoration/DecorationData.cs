using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "DecorationData", menuName = "CreateDecorationData")]
public class DecorationData : ScriptableObject
{
    // ================================
    // �����i�̃f�[�^
    // ================================


    // �����i�̃^�C�v
    public enum DecorationType
    {
        MAKURA,
        BED,

        MAX_DECO
    };

    // �����i�̎��
    [SerializeField] private DecorationType decoType;

    // �����i�̖��O
    [SerializeField ]private string decoName;

    // �v���C���[�ւ̕t�^�̗�
    [SerializeField] private int bufHP;
    // �v���C���[�ւ̕t�^�U����
    [SerializeField] private int bufATK;
    // �v���C���[�ւ̕t�^�h���
    [SerializeField] private int bufDEF;
    // �v���C���[�ւ̕t�^�^
    [SerializeField] private int bufLUCK;


    public DecorationType GetDecoType()
    {
        return decoType;
    }

    public string GetDecoName()
    {
        return decoName;
    }

    public int GetbufHP()
    {
        return bufHP;
    }
    public int GetbufATK()
    {
        return bufATK;
    }
    public int GetbufDEF()
    {
        return bufDEF;
    }
    public int GetbufLUCK()
    {
        return bufLUCK;
    }
}
