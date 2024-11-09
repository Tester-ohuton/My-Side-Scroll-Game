using UnityEngine;

public class AttackContoroll : MonoBehaviour
{
    bool hitflg;

    public float effectPosY;

    public GameObject damageEffect;

    EnemyStatus enemyStatus;
    
    private MyEnemy myEnemy;
    private EnemyInfo enemyInfo;
    private Enemy enemy;

    GameObject se;

	int AttackCnt = 1;

	void Start()
    {
        myEnemy = GameObject.Find("Actor").GetComponent<MyEnemy>();

        hitflg = false;

        se = GameObject.Find("SE");
    }

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        // オブジェクトタグがEnemyのとき
        if (!hitflg && collision.CompareTag("Enemy"))
        {
            if (se != null)
            {
                // 攻撃ヒット音
                se.GetComponent<SEManager>().PlaySE(1);
            }

            // 敵キャラを倒したかを取得
            enemyInfo = collision.gameObject.GetComponent<EnemyInfo>();
            
            enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy == null && enemy.IsHitFlag()) return;

            enemyStatus = collision.gameObject.GetComponent<EnemyStatus>();

            hitflg = true;

            // 敵が受けるダメージ（プレイヤーの攻撃力 - 敵の防御力）
            int damage = Mathf.Max(1, StaticStatus.GetPlayerATK() - enemyStatus.GetDEF());
            Debug.Log($"damage: {damage}");
            enemyStatus.SetHp(damage);

            if (enemyStatus.GetHp() <= 0)
            {
                if (!enemy.IsHitFlag())
                {
                    myEnemy.AddEnemy(enemyInfo.enemyData.GetEnemyType());
                    Debug.Log(enemyInfo.enemyData.GetEnemyType());
                    enemy.SetIsHitFlag(true);
                }
            }

            if (damageEffect != null)
            {
                GameObject effect = Instantiate(damageEffect) as GameObject;
                effect.transform.position = new Vector3(
                    this.gameObject.transform.position.x,
                    this.gameObject.transform.position.y + effectPosY,
                    this.gameObject.transform.position.z - 2.0f);
            }

            AttackCnt++;
        }
    }

    public void SethitFlg(bool flg)
    {
        hitflg = flg;
    }

    public bool GethitFlg()
    {
        return hitflg;
    }

    void OnTriggerExit(Collider t)
    {
        //Debug.Log("atattayo!!");
        //Hitflg = false;
    }
}