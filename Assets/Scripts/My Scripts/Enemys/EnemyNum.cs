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
        myEnemy = GameObject.Find("hime_Ani03").GetComponent<MyEnemy>();

        // オブジェクトからTextコンポーネントを取得
        enemyNum[0] = DispNum[0].GetComponent<Text>();
        enemyNum[1] = DispNum[1].GetComponent<Text>();
        enemyNum[2] = DispNum[2].GetComponent<Text>();
    }

    void Update()
    {
        // テキストの表示を入れ替える
        enemyNum[0].text = "でびあくま：" + myEnemy.GetStorage()[0] + $"/{enemyIndex[0]}";
        enemyNum[1].text = "おばけ：" + myEnemy.GetStorage()[1] + $"/{enemyIndex[1]}";
        enemyNum[2].text = "あざらし：" + myEnemy.GetStorage()[2] + $"/{enemyIndex[2]}";
    }
}
