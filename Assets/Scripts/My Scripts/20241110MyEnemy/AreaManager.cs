using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ内の各エリア管理クラス
/// </summary>
public class AreaManager : MonoBehaviour
{
    // オブジェクト・コンポーネント
    [HideInInspector] public StageManager stageManager; // ステージ管理クラス
    
    // エリア内敵データ配列
    EnemyBase[] inAreaEnemies;

    // 初期化関数(StageManager.csから呼出)
    public void Init(StageManager _stageManager)
    {
        // 参照取得
        stageManager = _stageManager;
        
        // エリア内の敵を取得&初期化
        inAreaEnemies = GetComponentsInChildren<EnemyBase>();
        foreach (var targetEnemyBase in inAreaEnemies)
            targetEnemyBase.Init(this);

        // アクターが進入するまでこのエリアを無効化
        gameObject.SetActive(false);
    }

    /// <summary>
    /// このエリアをアクティブ化する
    /// </summary>
    public void ActiveArea()
    {
        // 一旦全エリアを非アクティブ化
        stageManager.DeactivateAllAreas();

        // オブジェクト有効化
        gameObject.SetActive(true);

        // エリア内の敵をアクティブ化
        foreach (var targetEnemyBase in inAreaEnemies)
            targetEnemyBase.OnAreaActivated();
    }
}