using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticStatus : MonoBehaviour
{
    // =====================================
    // シーンを跨ぐステータス管理
    // =====================================
    // ステージ選択部屋の装飾品によって
    // ステータスが変動
    // 選択→ステータス保存→ゲームで使用
    // =====================================

    // --- 想定する使い方 ---
    // 選択シーンで装飾品に応じてステータス計算
    // SetStatusでステータス設定
    // ゲームシーンでGet関数を使い設定

    // 体力
    public static int PlayerHP = 100;

    // 攻撃力
    public static int PlayerATK = 10;

    // 防御力
    public static int PlayerDEF = 10;

    // 運
    public static int PlayerLUCK = 10;

    // 体力取得
    public static int GetPlayerHP()
    {
        return PlayerHP;
    }

    // 攻撃力取得
    public static int GetPlayerATK()
    {
        return PlayerATK;
    }

    // 防御力取得
    public static int GetPlayerDEF()
    {
        return PlayerDEF;
    }

    // 運取得
    public static int GetPlayerLUCK()
    {
        return PlayerLUCK;
    }

    // 選択シーンで装飾品に応じてステータス加算用
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
