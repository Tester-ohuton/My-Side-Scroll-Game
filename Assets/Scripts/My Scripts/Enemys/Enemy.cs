using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // ================================
    // ���G
    // ================================
    private DropItem item;
    private EnemyStatus sta;

    public bool isHitFlag;
    public bool isdead;

    public static bool flag = false;

    bool fade = false;

    public void Init()
    {
        item = GetComponent<DropItem>();
        sta = GetComponent<EnemyStatus>();
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
            // ���f���̏ꍇ�q�̃I�u�W�F�N�g�ɃX�N���v�g���A�^�b�`
            // ���Ă��邽�ߐe���Ə���
            // ���ǂ��炩
            //Destroy(gameObject.transform.parent.gameObject);

            Quest_Level_1.OnEnemyDestroyCountEvent.Invoke();

            Destroy(gameObject.transform.root.gameObject);

            if (gameObject.name == "obakefurosiki:obakefurosiki")
            {
                flag = true;
                Debug.Log("���΂��ӂ낵���͎���");
            }
            
            if (gameObject.name == "debiakuma")
            {
                flag = true;
                Debug.Log("�łт����܂͎���");
            }

            // ���݂͉��ŕ\�����Ă���I�u�W�F�N�g�ɃA�^�b�`���Ă��邽�߂�����폜
            //Destroy(gameObject);

            item.ItemDrop();
            isdead = false;
        }
    }
}
