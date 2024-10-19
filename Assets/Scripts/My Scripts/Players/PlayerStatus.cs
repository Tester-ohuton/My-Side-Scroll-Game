using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public GameObject ItemObject;
    public GameObject playerObj;
    // ================================
    // �v���C���[�X�e�[�^�X
    // ================================

    // �ő�HP
    public float maxHP;

    // ���݂�HP
    public float curHP;

    // �U����
    public float ATK;

    // �����
    public float DEF;

    // �^
    public float LUK;

    // SE�p
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
        // �f�o�b�O�p
        if(Input.GetKeyDown(KeyCode.F1))
        {
            SetMinusHp(50);
        }

        if (anime != null)
        {
            if (curHP <= 0)
            {
                // ���񂾃A�j���Đ�
                anime.SetBool("isDie", true);

                if (anime.GetCurrentAnimatorStateInfo(0).IsName("DieEnd"))
                {
                    // �v���C���[���A�N�e�B�u��
                    playerObj.SetActive(false);
                    // �搶��
                    Instantiate(ItemObject, transform.position, Quaternion.identity);
                    // �����蔻��Ȃ���
                    ItemObject.layer = LayerMask.NameToLayer("Invisible");

                    if (seobj != null)
                    {
                        // ���񂾂Ƃ�SE
                        seobj.GetComponent<SEManager>().PlaySE(0);
                    }
                }
            }
        }
    }


    // HP�����炷
    public void SetMinusHp(float hp)
    {
        this.curHP -= hp;
    }

    // ���݂�HP
    public float GetCurHp()
    {
        return curHP;
    }

    // �ő�HP
    public float GetMaxhp()
    {
        return maxHP;
    }

}
