using UnityEngine;
using UnityEngine.UI;

public class EnemyNum : MonoBehaviour
{
    // 敵ごとに数を表示するGameObject配列
    public GameObject[] DispNum = null;
    public int[] enemyIndex;

    private MyEnemy myEnemy;

    private Text[] enemyNum = new Text[(int)EnemyData.EnemyType.MAX_ENEMY];

    void Start()
    {
        myEnemy = GameObject.Find("Actor").GetComponent<MyEnemy>();

        // オブジェクトからTextコンポーネントを取得
        for (int i = 0; i < DispNum.Length; i++)
        {
            enemyNum[i] = DispNum[i].GetComponent<Text>();
        }

        /*
        enemyNum[0] = DispNum[0].GetComponent<Text>();
        enemyNum[1] = DispNum[1].GetComponent<Text>();
        enemyNum[2] = DispNum[2].GetComponent<Text>();
        enemyNum[3] = DispNum[3].GetComponent<Text>();
        enemyNum[4] = DispNum[4].GetComponent<Text>();
        enemyNum[5] = DispNum[5].GetComponent<Text>();
        */
    }

    void Update()
    {
        enemyNum[0].text = "敵の名前：" + "/" + myEnemy.GetStorage()[0] + $" / {enemyIndex[0]}";
        enemyNum[1].text = "敵の名前：" + "/" + myEnemy.GetStorage()[1] + $" / {enemyIndex[1]}";
        enemyNum[2].text = "敵の名前：" + "/" + myEnemy.GetStorage()[2] + $" / {enemyIndex[2]}";
        enemyNum[3].text = "敵の名前：" + "/" + myEnemy.GetStorage()[3] + $" / {enemyIndex[3]}";
        enemyNum[4].text = "敵の名前：" + "/" + myEnemy.GetStorage()[4] + $" / {enemyIndex[4]}";
        enemyNum[5].text = "敵の名前：" + "/" + myEnemy.GetStorage()[5] + $" / {enemyIndex[5]}";
        enemyNum[6].text = "敵の名前：" + "/" + myEnemy.GetStorage()[6] + $" / {enemyIndex[6]}";

        /*
        // テキストの表示を入れ替える
        enemyNum[0].text = "敵の名前1：" + myEnemy.GetStorage()[0] + $"/{enemyIndex[0]}";
        enemyNum[1].text = "敵の名前2：" + myEnemy.GetStorage()[1] + $"/{enemyIndex[1]}";
        enemyNum[2].text = "敵の名前3：" + myEnemy.GetStorage()[2] + $"/{enemyIndex[2]}";
        enemyNum[3].text = "敵の名前4：" + myEnemy.GetStorage()[3] + $"/{enemyIndex[3]}";
        enemyNum[4].text = "敵の名前5：" + myEnemy.GetStorage()[4] + $"/{enemyIndex[4]}";
        enemyNum[5].text = "敵の名前6：" + myEnemy.GetStorage()[5] + $"/{enemyIndex[5]}";
        */
    }
}
