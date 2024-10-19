using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // ================================
    // 敵関連のスクリプトをまとめて管理
    // ================================
    // 初期化されていないものが
    // でないようにするため

    // 敵オブジェクトにアタッチされているコンポーネント取得
    public EnemyInfo Info;
    public EnemyStatus Status;
    public DropItem Item;
    public Enemy enemy;


    
    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        Info = GetComponent<EnemyInfo>();
        Status = GetComponent<EnemyStatus>();
        Status.Init();
        Item = GetComponent<DropItem>();
        Item.Init();
        enemy = GetComponent<Enemy>();
        enemy.Init();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
