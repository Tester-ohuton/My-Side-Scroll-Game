using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticClear : MonoBehaviour
{
    // ================================
    // ステージクリア情報管理
    // ================================
    // --- 想定する使い方 ---
    // ステージ選択時StageNoに遊ぶステージ番号を設定
    // ステージクリア後
    // AddClearNumでClearNumを加算
    // 選択シーンにGetClearNumで反映
    // クリアしたステージ数 + 1ステージ解放
    // ※ゲーム開始時は１ステージ解放状態

    // 遊んでいるステージ番号
    public static int StageNo;

    // クリアステージ数
    public static int ClearNum = 0;


    public static int GetClearNum()
    {
        return ClearNum;
    }
    
    
    // クリア時加算
    public static void AddClearNum()
    {
        // 遊んでいるステージ番号がクリアしているステージ数より大きいときだけ加算
        // 未クリアのステージをクリアした時のみステージ解放
        if (StageNo > ClearNum)
        {
            ClearNum++;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
