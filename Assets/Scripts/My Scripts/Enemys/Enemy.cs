using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // ================================
    // 仮敵
    // ================================
    private DropItem item;
    private EnemyStatus sta;

    public bool isHitFlag;
    public bool isdead;

    public static bool flag = false;

    private Quest_Level_1 quest_Level_1;
    GameObject gameobj;

    bool fade = false;

    public void Init()
    {
        item = GetComponent<DropItem>();
        sta = GetComponent<EnemyStatus>();

        // 任意のオブジェクトを取得する
        gameobj = GameObject.Find("Quest");
        quest_Level_1 = gameobj.GetComponent<Quest_Level_1>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (sta.GetHp() <= 0)
        {
            //isdead = true;
            Die();
        }
        
     
    }

    public void SetIsHitFlag(bool flg)
    {
        isHitFlag = flg;
    }

    public bool IsHitFlag()
    {
        return isHitFlag;
    }

    public void SetIsDead(bool flg)
    {
        isdead = flg;
    }

    void Die()
    {
        if(isdead)
        {
            // モデルの場合子のオブジェクトにスクリプトをアタッチ
            // しているため親ごと消す
            // ↓どちらか
            //Destroy(gameObject.transform.parent.gameObject);

            if (quest_Level_1 != null)
            {
                // 各敵の倒した数を更新
                for (int i = 0; i < (int)EnemyData.EnemyType.MAX_ENEMY; ++i)
                {
                    quest_Level_1.EnemyEncountered((EnemyData.EnemyType)i);
                }
            }

            Destroy(gameObject.transform.root.gameObject);

            if (gameObject.name == "obakefurosiki:obakefurosiki")
            {
                flag = true;
                Debug.Log("おばけふろしきは死んだ");
            }
            
            if (gameObject.name == "debiakuma")
            {
                flag = true;
                Debug.Log("でびあくまは死んだ");
            }

            // 現在は仮で表示しているオブジェクトにアタッチしているためそれを削除
            //Destroy(gameObject);

            item.ItemDrop();
            isdead = false;
        }
    }
}
