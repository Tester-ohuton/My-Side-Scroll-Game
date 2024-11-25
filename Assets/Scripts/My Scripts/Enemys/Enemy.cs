using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // ================================
    // ���G
    // ================================
    [SerializeField] GameObject enemyPrefab;
    
    private DropItem item;
    private EnemyStatus sta;
    private MyEnemy myEnemy;
    private EnemyInfo enemyInfo;
    private GameObject player;

    public bool isdead;

    public static bool flag = false;

    bool fade = false;

    public void Init()
    {
        item = GetComponent<DropItem>();
        sta = GetComponent<EnemyStatus>();
        enemyInfo = GetComponent<EnemyInfo>();

        player = GameObject.Find("Actor");
        myEnemy = player.GetComponent<MyEnemy>();
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

    public void SetIsDead(bool flg)
    {
        isdead = flg;
    }

    public bool GetDead()
    {
        return isdead;
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

            
            myEnemy.AddEnemy(enemyInfo.enemyData.GetEnemyType());
            Debug.Log(enemyInfo.enemyData.GetEnemyType());

            enemyPrefab.SetActive(false);
            sta.Init();

            //Destroy(gameObject.transform.root.gameObject);

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

    public void ReSpawn()
    {
        enemyPrefab.SetActive(true);
    }
}
