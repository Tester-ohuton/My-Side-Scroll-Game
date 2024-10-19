using UnityEngine;
using UnityEngine.UI;

public class EnemyNum : MonoBehaviour
{
    // �G���Ƃɐ���\������GameObject�z��
    public GameObject[] DispNum = null;

    public int[] enemyIndex;

    private MyEnemy myEnemy;

    private Text[] enemyNum = new Text[(int)EnemyData.EnemyType.MAX_ENEMY];

    void Start()
    {
        myEnemy = GameObject.Find("hime_Ani03").GetComponent<MyEnemy>();

        // �I�u�W�F�N�g����Text�R���|�[�l���g���擾
        enemyNum[0] = DispNum[0].GetComponent<Text>();
        enemyNum[1] = DispNum[1].GetComponent<Text>();
        enemyNum[2] = DispNum[2].GetComponent<Text>();
    }

    void Update()
    {
        // �e�L�X�g�̕\�������ւ���
        enemyNum[0].text = "�łт����܁F" + myEnemy.GetStorage()[0] + $"/{enemyIndex[0]}";
        enemyNum[1].text = "���΂��F" + myEnemy.GetStorage()[1] + $"/{enemyIndex[1]}";
        enemyNum[2].text = "�����炵�F" + myEnemy.GetStorage()[2] + $"/{enemyIndex[2]}";
    }
}
