using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ItemNum : MonoBehaviour
{
    // ================================
    // 所持アイテム表示UI
    // ================================

    public GameObject[] DispNum = null;

    private MyItem myitem;

    private Text[] itemNum = new Text[(int)ItemData.Type.MAX_ITEM];

    // Start is called before the first frame update
    void Start()
    {
        myitem = GameObject.Find("Actor").GetComponent<MyItem>();

        // オブジェクトからTextコンポーネントを取得
        itemNum[0] = DispNum[0].GetComponent<Text>();
        itemNum[1] = DispNum[1].GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // テキストの表示を入れ替える
        itemNum[0].text = "綿：" +  myitem.GetStorage()[0] + "/4";
        itemNum[1].text = "布：" + myitem.GetStorage()[1] + "/2";
    }
}
