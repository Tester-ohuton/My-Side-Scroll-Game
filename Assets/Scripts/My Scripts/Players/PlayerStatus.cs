using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public GameObject ItemObject;
    public GameObject playerObj;
    // ================================
    // プレイヤーステータス
    // ================================

    // 最大HP
    public float maxHP;

    // 現在のHP
    public float curHP;

    // 攻撃力
    public float ATK;

    // 守備力
    public float DEF;

    // 運
    public float LUK;

    // SE用
    GameObject seobj;

    Animator anime;

    // Start is called before the first frame update
    void Start()
    {
        curHP = maxHP = StaticStatus.GetPlayerHP();
        ATK = StaticStatus.GetPlayerATK();
        DEF = StaticStatus.GetPlayerDEF();
        LUK = StaticStatus.GetPlayerLUCK();

        seobj = GameObject.Find("SE");

        //Debug.Log(maxHP);
        //Debug.Log(ATK);
        //Debug.Log(DEF);
        //Debug.Log(LUK);

        anime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // デバッグ用
        if(Input.GetKeyDown(KeyCode.F1))
        {
            SetMinusHp(50);
        }

        if (anime != null)
        {
            if (curHP <= 0)
            {
                // 死んだアニメ再生
                anime.SetBool("isDie", true);

                if (anime.GetCurrentAnimatorStateInfo(0).IsName("DieEnd"))
                {
                    // プレイヤーを非アクティブ化
                    playerObj.SetActive(false);
                    // 墓生成
                    Instantiate(ItemObject, transform.position, Quaternion.identity);
                    // 当たり判定なくす
                    ItemObject.layer = LayerMask.NameToLayer("Invisible");

                    if (seobj != null)
                    {
                        // 死んだときSE
                        seobj.GetComponent<SEManager>().PlaySE(0);
                    }
                }
            }
        }
    }


    // HPを減らす
    public void SetMinusHp(float hp)
    {
        this.curHP -= hp;
    }

    // 現在のHP
    public float GetCurHp()
    {
        return curHP;
    }

    // 最大HP
    public float GetMaxhp()
    {
        return maxHP;
    }

}
