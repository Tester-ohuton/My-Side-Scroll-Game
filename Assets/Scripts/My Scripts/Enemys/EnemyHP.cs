using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{

    [SerializeField]
    private float hp = 5;  //体力
    int cube = 0;
    void Start()
    {
    }

    // Start is called before the first frame update
    void OnCollisionEnter(Collision collision)
    {
        //タグがEnemyBulletのオブジェクトが当たった時に{}内の処理が行われる
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("hit Player");  //コンソールにhit Playerが表示
            //gameObject.GetComponent<EnemyBulletManager>()でEnemyBulletManagerスクリプトを参照し
            //.powerEnemy; でEnemyBulletManagerのpowerEnemyの値をゲット
            hp -= collision.gameObject.GetComponent<PlayerStatus>().ATK;
        }

        //体力が0以下になった時{}内の処理が行われる
        if (hp <= 0)
        {
            Destroy(gameObject);
            FadeManager.Instance.LoadScene("Result", 2.0f);
            cube++;
            if (cube == 2)
            {

            }
        }
    }



}


    
