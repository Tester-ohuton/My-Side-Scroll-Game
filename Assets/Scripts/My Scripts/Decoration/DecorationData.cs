using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "DecorationData", menuName = "CreateDecorationData")]
public class DecorationData : ScriptableObject
{
    // ================================
    // 装飾品のデータ
    // ================================


    // 装飾品のタイプ
    public enum DecorationType
    {
        MAKURA,
        BED,

        MAX_DECO
    };

    // 装飾品の種類
    [SerializeField] private DecorationType decoType;

    // 装飾品の名前
    [SerializeField ]private string decoName;

    // プレイヤーへの付与体力
    [SerializeField] private int bufHP;
    // プレイヤーへの付与攻撃力
    [SerializeField] private int bufATK;
    // プレイヤーへの付与防御力
    [SerializeField] private int bufDEF;
    // プレイヤーへの付与運
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
