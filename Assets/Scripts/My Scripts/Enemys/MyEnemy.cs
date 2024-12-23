using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemy : MonoBehaviour
{
    const int Num = (int)EnemyData.EnemyType.MAX_ENEMY;

    // 取得アイテム格納用配列
    int[] Storage = new int[Num];

    bool fadestart = false;

    void Start()
    {
        // 配列の要素数分初期化
        for (int i = 0; i < Storage.Length; ++i)
        {
            // 全て０
            Storage[i] = 0;
        }
    }


    void Update()
    {
        // hpが0でないときに常にfalse
        StaticEnemy.IsUpdate = false;

        /*
        // 仮
        if (Storage[0] == 2)
        {
            if (!fadestart)
            {
                fadestart = true;
                FadeManager.Instance.LoadScene("Result", 2.0f);
                Debug.Log("a");
                //fadestart = false;
            }
        }
        */
    }

    // 敵キャラ格納
    // @param...type 格納する敵キャラの種類
    public void AddEnemy(EnemyData.EnemyType type)
    {
        // 引数で渡された敵キャラの数を増やす
        Storage[(int)type]++;
        Debug.Log($"Storage: {Storage[(int)type]}");
    }

    // ストレージの中身を取得
    public int[] GetStorage()
    {
        return Storage;
    }
}
