using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationManager : MonoBehaviour
{
    // ================================
    // 装飾品管理
    // ================================
    // 装飾品の種類数
    const int Deco_NUM = (int)DecorationData.DecorationType.MAX_DECO;

    // 装飾品格納用
    private GameObject[] Decoration = new GameObject[Deco_NUM];

    // 装飾品が作られているかどうか(添え字で種類を判断)
    private bool[] IsCreate = new bool[Deco_NUM];

    // 装飾品の情報取得用
    DecorationInfo[] decoInfo = new DecorationInfo[Deco_NUM];

    // Start is called before the first frame update
    void Start()
    {
        // 最初の１回だけstatic配列をfalseで初期化
        if(!StaticDecoration.isInit)
        {
            // 保持用の作成フラグを初期化false
            // 保持用のステータス加算フラグ初期化
            StaticDecoration.InitFlag();
            // 初期化終了したら初期化完了フラグオン
            StaticDecoration.isInit = true;

            StaticItem.InitWarehouse();

            StaticEnemy.InitWarehouse();
        }

        // 装飾品が作られているかどうか
        for(int i = 0;i<IsCreate.Length;++i)
        {
            // 保持している作成フラグを取得
            IsCreate[i] = StaticDecoration.GetIsCreate()[i];

            // 作成済みの場合リソースフォルダからプレハブ読み込み
            if(IsCreate[i])
            {
                // 装飾品読み込み
                LoadDeco(i);
                // 設置
                Instantiate(Decoration[i], new Vector3(0, 3, -10), Quaternion.identity);
            }
            else
            {
                // 作成済みでなければnull
                Decoration[i] = null;
            }
        }


        Debug.Log("綿" + StaticItem.GetItemWarehouse()[0]);
        Debug.Log("布" + StaticItem.GetItemWarehouse()[1]);
    }

    // Update is called once per frame
    void Update()
    {
        // まくら作成
        // 作成済みフラグオフのとき
        if(IsCreate[0] && Decoration[0] == null)
        {
            // プレハブ読み込み
            LoadDeco(0);
            // 読み込んだプレハブを生成
            CreateDeco(0);
            // プレイヤーのステータスに加算する
            SetBufStatus();
        }

        if (IsCreate[1] && Decoration[1] == null)
        {
            LoadDeco(1);
            CreateDeco(1);
            SetBufStatus();
        }
    }

    // プレハブ読み込み関数
    void LoadDeco(int no)
    {
        // 番号ごとに読み込むプレハブ変更
        switch (no)
        {
            case 0:
                Decoration[no] = (GameObject)Resources.Load("Prefab/makura");
                break;

            case 1:
                Decoration[no] = (GameObject)Resources.Load("Prefab/bed");
                break;
        }
    }

    public void SetIsCreate(int no, bool flg)
    {
        IsCreate[no] = flg;
    }

    void CreateDeco(int no)
    {
        // 作成フラグオン
        // 装飾品がつくられたら
        IsCreate[no] = true;
        // 作成フラグ保持内容更新
        StaticDecoration.SetIsCreate(IsCreate);
        // オブジェクト（装飾品）作成
        Instantiate(Decoration[no], new Vector3(0, 3, -10), Quaternion.identity);
        // 作成した装飾品の情報取得
        decoInfo[no] = Decoration[no].GetComponent<DecorationInfo>();
    }

    // 装飾品に応じてプレイヤーステータスを設定
    // プレイヤーのステータスを保持するstaticな変数へ設定
    public void SetBufStatus()
    {
        for(int i = 0;i<IsCreate.Length;++i)
        {
            // 作られていない、すでにステータス加算済み、装飾品情報がNULLの場合とばす
            if (!IsCreate[i] || StaticDecoration.GetIsAdd()[i] || !decoInfo[i]) continue;

            StaticStatus.SetStatus(
                    decoInfo[i].decoData.GetbufHP(),
                    decoInfo[i].decoData.GetbufATK(),
                    decoInfo[i].decoData.GetbufDEF(),
                    decoInfo[i].decoData.GetbufLUCK());

            // 加算済みフラグオン
            StaticDecoration.GetIsAdd()[i] = true;
            
        }
    }
}
