using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{
    // ================================
    // 敵の情報を取得してステータス設定
    // ================================

    // エネミーの情報取得用
    EnemyInfo enemyinfo;

    // 敵のMaxHP
    [SerializeField] private int maxHP;

    // 敵の現在のHP
    [SerializeField] private int curHP;

    // 敵の攻撃力
    [SerializeField] int ATK;

    [SerializeField] int DEF;

    // HP表示用UI
    [SerializeField] private GameObject HPUI;

    // HP表示用スライダー
    private Slider hpSlider;

    
    public void Init()
    {
        // 敵のデータからステータスを読み取り設定
        enemyinfo = GetComponent<EnemyInfo>();
        curHP = maxHP = enemyinfo.enemyData.GetHp();
        ATK = enemyinfo.enemyData.GetATK();
        DEF = enemyinfo.enemyData.GetDEF();
        HPUI.SetActive(true);
        hpSlider = HPUI.transform.Find("HPBar").GetComponent<Slider>();
        hpSlider.value = 1.0f;

    }

    // Update is called once per frame
    void Update()
    {
        // 仮：ZキーでHP現象
        if(Input.GetKeyUp(KeyCode.Z))
        {
            SetHp(20);            
        }
    }

    public void SetHp(int hp)
    {
        this.curHP -= hp;

        // HP表示用UIのアップデート
        UpdateHPValue();
    }

    public int GetHp()
    {
        return curHP;
    }

    public int GetMaxhp()
    {
        return maxHP;
    }

    public int GetATK()
    {
        return ATK;
    }

    public int GetDEF()
    {
        return DEF;
    }

    // 死んだらHPUIを非表示にする
    public void HideStatusUI()
    {
        HPUI.SetActive(false);
    }

    public void UpdateHPValue()
    {
        hpSlider.value = (float)GetHp() / (float)GetMaxhp();
    }  
}
