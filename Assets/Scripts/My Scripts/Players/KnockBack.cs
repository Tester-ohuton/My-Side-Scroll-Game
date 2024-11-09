using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    // プレイヤーステータス取得用
    private PlayerStatus playerStatus;
    // 接触した瞬間のプレイヤー座標
    Vector3 prePos;
    // 接触した敵の位置取得
    Vector3 enemyPos;
    // ノックバック処理始まっているか否か
    bool isStart = false;
    // ノックバック処理のステップ(何番目の処理か)
    int Step = 0;

    // 敵のステータス取得用
    EnemyStatus enemyStatus;

    //CharacterController controller;
    Rigidbody2D rb2D;

    // 操作不可時間
    float inoperableTime = 1.0f;

    // 操作不可フラグ
    bool isInoperable = false;

    // --- 点滅用 ---
    // 子のRendererの配列
    Renderer[] childrenRenderer;

    SpriteRenderer spriteRenderer;

    // 今childRendererが有効か無効化のフラグ
    bool isEnabledRenderers;

    // 今ダメージを受けているかのフラグ
    bool isDamaged;

    // リセットする時の為にコルーチンを保持しておく
    Coroutine flicker;

    // ダメージの点滅の長さ。
    float flickerDuration = 2.0f;

    // 無敵時間
    float invincibleTime;

    // ダメージ点滅の合計経過時間
    float flickerTotalElapsedTime;
    // ダメージ点滅のRendererの有効・無効切り替え用の経過時間
    float flickerElapsedTime;

    // ダメージ点滅のRendererの有効・無効切り替え用のインターバル
    float flickerInterval = 0.075f;

    // ノックバック時間(これ以上の時間が経過したら自動で次のステップ)
    float KnockTime = 0.0f;

    Animator anime;

    GameObject se;

    // Start is called before the first frame update
    void Start()
    {
        // 点滅時間と無敵時間を共有
        invincibleTime = flickerDuration;

        playerStatus = GetComponent<PlayerStatus>();
        rb2D = GetComponent<Rigidbody2D>();

        childrenRenderer = GetComponentsInChildren<Renderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        anime = GetComponent<Animator>();

        se = GameObject.Find("SE");
    }

    // Update is called once per frame
    void Update()
    {
        // ノックバック処理スタート
        if (isStart && !anime.GetCurrentAnimatorStateInfo(0).IsName("Knock"))
        {
            knockback();
        }
    }


    // ノックバック関数
    public void knockback()
    {
        switch (Step)
        {
            // プレイヤーのHPを減らす
            case 0:
                anime.SetBool("isKnock", true);
                // プレイヤーが受けるダメージ（敵の攻撃力 - プレイヤーの防御力）
                // 受けるダメージ０より大きい時
                
                if (enemyStatus.GetATK() - StaticStatus.GetPlayerDEF() > 0)
                {
                    //Debug.Log(enemyStatus.GetATK() - StaticStatus.GetPlayerDEF());
                    playerStatus.SetMinusHp(enemyStatus.GetATK() - StaticStatus.GetPlayerDEF());
                }
                // ０以下なら最低１ダメージ
                else
                {
                    playerStatus.SetMinusHp(1);
                }

                Step++;
                break;

            // プレイヤー仰け反る（ヒットストップ？）
            case 1:

                spriteRenderer.color = new Color(0.5f, 0, 0, 1f);

                // レイヤーをInvisibleに変更(当たり判定をなくす)
                this.gameObject.layer = LayerMask.NameToLayer("Invisible");

                // 操作不可フラグをオン
                isInoperable = true;

                // 自分の位置と接触したオブジェクトの位置を計算して
                // 距離と方向を出して正規化
                Vector3 distination = new Vector3((this.transform.position.x - enemyPos.x), 0, 0).normalized;

                if (Mathf.Abs(prePos.x - this.transform.position.x) < 1)
                {
                    // ノックバック
                    Knock(distination.x);

                    // 壁に詰まったときよう...強制的に次のステップへ
                    KnockTime += Time.deltaTime;
                    if (KnockTime > 1.0f)
                    {
                        Step++;
                    }
                }
                else
                {
                    // 移動完了したら次のステップへ
                    Step++;
                    anime.SetBool("isKnock", false);
                }



                break;
            // 点滅
            case 2:
                if (isDamaged)
                    return;
                StartFlicker();
                Step++;
                break;

            // 無敵時間(2秒、ダメージ硬直1秒込み)
            case 3:
                if (0 < invincibleTime)
                {
                    invincibleTime -= Time.deltaTime;
                    if(0 < inoperableTime)
                    {
                        inoperableTime -= Time.deltaTime;
                    }
                    else
                    {
                        // １秒経ったら操作不可フラグオフ
                        isInoperable = false;
                    }
                }
                else
                {
                    Step++;
                }
                break;

            
            case 4:
                // ノックバック処理終了
                isStart = false;
                // 処理順を最初に戻す
                Step = 0;
                // レイヤーをPlayerに戻す
                this.gameObject.layer = LayerMask.NameToLayer("Player");
                // 無敵時間初期化
                invincibleTime = 2.0f;
                // 操作不可時間初期化
                inoperableTime = 1.0f;
                // 強制で次のステップへ
                KnockTime = 0.0f;
                break;
        }

    }


    void SetEnabledRenderers(bool b)
    {
        for (int i = 0; i < childrenRenderer.Length; i++)
        {
            childrenRenderer[i].enabled = b;
        }
    }

    void StartFlicker()
    {
        flicker = StartCoroutine("Flicker");
    }


    IEnumerator Flicker()
    {
        isDamaged = true;

        flickerTotalElapsedTime = 0;
        flickerElapsedTime = 0;

        while (true)
        {
            flickerTotalElapsedTime += Time.deltaTime;
            flickerElapsedTime += Time.deltaTime;

            if (flickerInterval <= flickerElapsedTime)
            {
                //ここが被ダメージ点滅の処理。
                flickerElapsedTime = 0;
                //Rendererの有効、無効の反転。
                isEnabledRenderers = !isEnabledRenderers;
                SetEnabledRenderers(isEnabledRenderers);
            }


            if (flickerDuration <= flickerTotalElapsedTime)
            {
                spriteRenderer.color = new Color(1, 1, 1, 1);

                //ここが被ダメージ点滅の終了時の処理。
                isDamaged = false;

                //最後には必ずRendererを有効にする(消えっぱなしになるのを防ぐ)。
                isEnabledRenderers = true;
                SetEnabledRenderers(true);

                yield break;
            }
            yield return null;
        }
    }

    //コルーチンのリセット用。
    void ResetFlicker()
    {
        if (flicker != null)
        {
            StopCoroutine(flicker);
            flicker = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" && !isStart)
        {
            if (se != null)
            {
                // 攻撃ヒット音
                se.GetComponent<SEManager>().PlaySE(2);
            }

            // プレイヤーが止まっている時
            // 敵と衝突
            isStart = true;
            // 接触時敵とプレイヤーの座標取得
            enemyPos = other.gameObject.transform.position;
            prePos = this.transform.position;
            // 止まっている時はrigidbodyがついているオブジェクトと当たり判定を取っている
            // 上記の子オブジェクトにステータス情報があるため子からGetComponent
            enemyStatus = other.gameObject.GetComponentInChildren<EnemyStatus>();
            
        }
    }

    private void Knock(float knockX)
    {
        // ノックバックの力をRigidbodyに加える
        rb2D.velocity = Vector2.zero; // 現在の速度をリセット
        rb2D.AddForce(new Vector2(knockX, 0), ForceMode2D.Impulse);
    }

    public bool GetIsInoperable()
    {
        return isInoperable;
    }
}
