using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 全エネミー共通処理クラス
/// </summary>
public class EnemyBase : MonoBehaviour
{
    // オブジェクト・コンポーネント
    [HideInInspector] public AreaManager areaManager; // エリアマネージャ
    protected Rigidbody rb; // Rigidbody/
    protected Transform actorTransform; // 主人公(アクター)のTransform
    protected Image bossHPGage; // ボス用HPゲージ
    protected EnemyStatus enemyStatus;

    // 各種変数
    // 基礎データ(インスペクタから入力)
    [Header("最大体力(初期体力)")]
    protected int maxHP;
    [Header("接触時アクターへのダメージ")]
    public int touchDamage;
    [Header("ボス敵フラグ(ONでボス敵として扱う。１ステージに１体のみ)")]
    public bool isBoss;
    [Header("ボス用被撃破パーティクルPrefab")]
    public GameObject bossDefeatParticlePrefab;
    // その他データ
    [HideInInspector] public int nowHP; // 残りHP

    public int GetHp()
    {
        return nowHP;
    }

    [HideInInspector] public bool isDamaged;

    public void SetIsDamaged(bool flag)
    {
        isDamaged = flag;
    }

    [HideInInspector] public bool isDead;

    public void SetIsDead(bool flag)
    {
        isDead = flag;
    }

    [HideInInspector] public bool isVanishing; // 消滅中フラグ trueで消滅中である
    [HideInInspector] public bool isInvis; // 無敵モード
    [HideInInspector] public bool rightFacing; // 右向きフラグ(falseで左向き)

    // DoTween用
    private Tween damageTween;  // 被ダメージ時演出Tween

    // 定数定義
    private readonly Color COL_DEFAULT = new Color(1.0f, 1.0f, 1.0f, 1.0f);    // 通常時カラー
    private readonly Color COL_DAMAGED = new Color(1.0f, 0.1f, 0.1f, 1.0f);    // 被ダメージ時カラー
    private const float KNOCKBACK_X = 1.8f; // 被ダメージ時ノックバック力(x方向)
    private const float KNOCKBACK_Y = 0.3f; // 被ダメージ時ノックバック力(y方向)

    // 初期化関数(AreaManager.csから呼出)
    public void Init(AreaManager _areaManager)
    {
        // 参照取得
        areaManager = _areaManager;
        actorTransform = areaManager.stageManager.player.transform;
        rb = GetComponent<Rigidbody>();

        enemyStatus = this.transform.GetChild(0).GetComponent<EnemyStatus>();

        // 変数初期化
        rb.freezeRotation = true;
        nowHP = maxHP = enemyStatus.GetHp();
        if (transform.localScale.x > 0.0f)
            rightFacing = true;

        // エリアがアクティブになるまで何も処理せず待機
        gameObject.SetActive(false);
    }
    /// <summary>
    /// このモンスターの居るエリアにアクターが進入した時の処理(エリアアクティブ化時処理)
    /// </summary>
    public virtual void OnAreaActivated()
    {
        // このモンスターをアクティブ化
        gameObject.SetActive(true);

        // ボス敵用処理
        if (isBoss)
        {
            // HPゲージ表示
            areaManager.stageManager.bossHPGage.transform.parent.gameObject.SetActive(true);
            bossHPGage = areaManager.stageManager.bossHPGage;
            bossHPGage.fillAmount = 1.0f;
            // BGM再生
            areaManager.stageManager.PlayBossBGM();
        }
    }

    /// <summary>
    /// ダメージを受ける際に呼び出される
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    /// <returns>ダメージ成功フラグ trueで成功</returns>
    public bool Damaged(int damage)
    {
        // ダメージ処理
        nowHP -= damage;
        // (ボス用)HPゲージの表示を更新する
        if (bossHPGage != null)
        {
            float hpRatio = (float)nowHP / maxHP;
            bossHPGage.DOFillAmount(hpRatio, 0.5f);
        }

        if (nowHP <= 0.0f)
        {// HP0の場合
         // 被ダメージTween初期化
            if (damageTween != null)
                damageTween.Kill();
            damageTween = null;

            // 消滅中フラグをセット
            isVanishing = true;
            // 消滅中は物理演算なし
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
            
            // その他撃破時処理
            if (isBoss)
            {// ボス撃破時
             // ボス撃破パーティクルを生成
                var obj = Instantiate(bossDefeatParticlePrefab);
                obj.transform.position = transform.position;
                // ゲームクリア処理
                areaManager.stageManager.StageClear();
            }
            else
            {// ザコ撃破時
            }
        }
        else
        {// まだHPが残っている場合
         // 被ダメージTween初期化
            if (damageTween != null)
                damageTween.Kill();
            damageTween = null;
            // 被ダメージ演出再生

        }

        return true;
    }
    /// <summary>
    /// エネミーが消滅する際に呼び出される
    /// </summary>
    private void Vanish()
    {
        // オブジェクト消滅
        Destroy(gameObject);
    }

    /// <summary>
    /// アクターに接触ダメージを与える処理
    /// </summary>
    public void BodyAttack(GameObject actorObj)
    {
        // 自身が消滅中なら無効
        if (isVanishing)
            return;

        // アクターのコンポーネントを取得
        PlayerStatus playerStatus = actorObj.GetComponent<PlayerStatus>();
        if (playerStatus == null)
            return;

        // アクターに接触ダメージを与える
        playerStatus.SetMinusHp(touchDamage);
    }

    /// <summary>
    /// オブジェクトの向きを左右で決定する
    /// </summary>
    /// <param name="isRight">右向きフラグ</param>
    public void SetFacingRight(bool isRight)
    {
        // キャラクターの向き制御
        Vector2 lscale = gameObject.transform.localScale;

        if (!isRight)
        {// 左向き
            lscale.x *= -1;

            // 右向きフラグoff
            rightFacing = false;
        }
        else
        {// 右向き
            lscale.x *= 1;

            // 右向きフラグon
            rightFacing = true;
        }
    }
}