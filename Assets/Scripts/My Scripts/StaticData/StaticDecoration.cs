using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDecoration : MonoBehaviour
{
    // ================================
    // 装飾品情報保持用
    // ================================

    // 初期化したかどうか
    public static bool isInit = false;

    // 装飾品の情報格納配列
    // 添え字番目の装飾品が作られたかどうか格納
    public static bool[] IsCreate = new bool[(int)DecorationData.DecorationType.MAX_DECO];
    
    // ステータス加算済みかどうか
    public static bool[] IsAdd = new bool[(int)DecorationData.DecorationType.MAX_DECO];

    public static void InitFlag()
    {
        for (int i = 0; i < IsCreate.Length; ++i)
        {
            IsCreate[i] = false;
            IsAdd[i] = false;
        }
    }

    // staticな加算フラグ配列を取得
    public static bool[] GetIsAdd()
    {
        return IsAdd;
    }


    
    
    public static void SetIsCreate(bool[] iscreate)
    {
        for (int i = 0; i < IsCreate.Length; ++i)
        {
            IsCreate[i] = iscreate[i];
        }
    }

    public static bool[] GetIsCreate()
    {
        return IsCreate;
    }


    public static void AddDecoration()
    {

    }

}
