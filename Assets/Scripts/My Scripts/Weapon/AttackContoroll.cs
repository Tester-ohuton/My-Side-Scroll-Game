//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class AttackContoroll : MonoBehaviour
{
    bool Hitflg;

    public float EffectPosY;

    public GameObject DamageEffect;

    EnemyStatus enemyStatus;
    
    private MyEnemy myEnemy;
    private EnemyInfo enemyinfo;
    private Enemy enemy;

    GameObject se;

	int AttackCnt = 1;

	// Start is called before the first frame update
	void Start()
    {
        myEnemy = GameObject.Find("hime_Ani03").GetComponent<MyEnemy>();

        Hitflg = false;

        se = GameObject.Find("SE");
    }

    // Update is called once per frame
    void Update()
    {
		if (AttackCnt == 1)
		{
        }
		if (AttackCnt == 2)
		{
        }
		if (AttackCnt == 3)
		{
			AttackCnt = 0;
		}
	}

    void OnTriggerEnter(Collider t)
    {        
        // オブジェクトタグがEnemyのとき
        if (!Hitflg && t.gameObject.CompareTag("Enemy"))
        {
            // 攻撃ヒット音
            se.GetComponent<SEManager>().PlaySE(1);

            // 敵キャラを倒したかを取得
            //if()
            enemyinfo = t.gameObject.GetComponent<EnemyInfo>();
            
            enemy = t.gameObject.GetComponent<Enemy>();
            if (enemy != null && enemy.IsHitFlag()) return;
            if (enemy.isHitFlag) return;

            enemyStatus = t.gameObject.GetComponent<EnemyStatus>();

            Hitflg = true;

            // 敵が受けるダメージ（プレイヤーの攻撃力 - 敵の防御力）
            int damage = Mathf.Max(1, StaticStatus.GetPlayerATK() - enemyStatus.GetDEF());
            enemyStatus.SetHp(damage);

            if (enemyStatus.GetHp() <= 0)
            {
                if (!enemy.IsHitFlag())
                {
                    myEnemy.AddEnemy(enemyinfo.enemyData.GetEnemyType());
                    enemy.SetIsHitFlag(true);
                }
            }

            GameObject effect = Instantiate(DamageEffect) as GameObject;
            effect.transform.position = new Vector3(
                this.gameObject.transform.position.x,
                this.gameObject.transform.position.y + EffectPosY,
                this.gameObject.transform.position.z - 2.0f);

            AttackCnt++;
        }
    }

    public void SethitFlg(bool flg)
    {
        Hitflg = flg;
    }

    public bool GethitFlg()
    {
        return Hitflg;
    }

    void OnTriggerExit(Collider t)
    {
        //Debug.Log("atattayo!!");
        //Hitflg = false;
    }
}