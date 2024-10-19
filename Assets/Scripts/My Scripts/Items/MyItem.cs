using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MyItem : MonoBehaviour
{
    // ================================
    // プレイヤーの所持アイテム
    // ================================


    const int Num = (int)ItemData.Type.MAX_ITEM;

    // 取得アイテム格納用配列
    int[] Storage = new int[Num];


    bool fadestart = false;

    // Start is called before the first frame update
    void Start()
    {
        // 配列の要素数分初期化
        for(int i = 0;i<Storage.Length;++i)
        {
            // 全て０
            Storage[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // アイテムに触っていないとき常にfalse
        // 更新しちゃダメ
        StaticItem.IsUpdate = false;
        // 仮
        //if (Storage[0] == 2)
        //{
        //    if (!fadestart)
        //    {
        //        fadestart = true;
        //        FadeManager.Instance.LoadScene("Result", 2.0f);
        //        Debug.Log("a");
        //        //fadestart = false;
        //    }
            
        //}
       
    }

    // アイテム格納
    // @param...type 格納するアイテムの種類
    public void AddItem(ItemData.Type type)
    {
        // 引数で渡されたアイテムの数を増やす
        Storage[(int)type] += 1;
        Debug.Log(Storage[(int)type]);
    }

    // ストレージの中身を取得
    public int[] GetStorage()
    {
        return Storage;
    }
}
