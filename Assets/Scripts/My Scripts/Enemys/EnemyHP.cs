using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{

    [SerializeField]
    private float hp = 5;  //�̗�
    int cube = 0;
    void Start()
    {
    }

    // Start is called before the first frame update
    void OnCollisionEnter(Collision collision)
    {
        //�^�O��EnemyBullet�̃I�u�W�F�N�g��������������{}���̏������s����
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("hit Player");  //�R���\�[����hit Player���\��
            //gameObject.GetComponent<EnemyBulletManager>()��EnemyBulletManager�X�N���v�g���Q�Ƃ�
            //.powerEnemy; ��EnemyBulletManager��powerEnemy�̒l���Q�b�g
            hp -= collision.gameObject.GetComponent<PlayerStatus>().ATK;
        }

        //�̗͂�0�ȉ��ɂȂ�����{}���̏������s����
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


    
