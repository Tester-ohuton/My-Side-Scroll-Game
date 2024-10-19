using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlayer : MonoBehaviour
{
    // =====================================
    // プレイヤーに向かって移動(アイテム)
    // =====================================

    // プレイヤー座標取得用
    Player playerPos;

    // スクリプトをアタッチしたオブジェクトからプレイヤー
    // に向けてのベクトル格納用
    Vector3 vec;

    // ２点間の距離格納用
    float distance;

    // 移動開始範囲
    public int range = 5;

    // スピード
    private float speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("hime_Ani03").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        

        // ２点間の距離を求める
        distance = Vector3.Distance(transform.position, playerPos.transform.position);
        

        vec = playerPos.transform.position - this.transform.position;
        vec = vec.normalized;

        // 2点間の距離が値以下だったらプレイヤーに向かって移動
        if (distance <= range)
        {

            transform.position += Time.deltaTime * vec * speed;

        }
    }
}
