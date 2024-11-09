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
        myEnemy = GameObject.Find("Actor").GetComponent<MyEnemy>();

        // �I�u�W�F�N�g����Text�R���|�[�l���g���擾
        enemyNum[0] = DispNum[0].GetComponent<Text>();
        enemyNum[1] = DispNum[1].GetComponent<Text>();
        enemyNum[2] = DispNum[2].GetComponent<Text>();
        enemyNum[3] = DispNum[3].GetComponent<Text>();
        enemyNum[4] = DispNum[4].GetComponent<Text>();
        enemyNum[5] = DispNum[5].GetComponent<Text>();
    }

    void Update()
    {
        // �e�L�X�g�̕\�������ւ���
        enemyNum[0].text = "1�F" + myEnemy.GetStorage()[0] + $"/{enemyIndex[0]}";
        enemyNum[1].text = "2�F" + myEnemy.GetStorage()[1] + $"/{enemyIndex[1]}";
        enemyNum[2].text = "3�F" + myEnemy.GetStorage()[2] + $"/{enemyIndex[2]}";
        enemyNum[3].text = "4�F" + myEnemy.GetStorage()[3] + $"/{enemyIndex[3]}";
        enemyNum[4].text = "5�F" + myEnemy.GetStorage()[4] + $"/{enemyIndex[4]}";
        enemyNum[5].text = "6�F" + myEnemy.GetStorage()[5] + $"/{enemyIndex[5]}";
    }
}
