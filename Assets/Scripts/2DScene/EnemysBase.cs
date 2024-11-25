using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemysBase : MonoBehaviour
{
    public float moveSpeed = 2f;

    // オブジェクト・コンポーネント
    [HideInInspector] public AreaManager areaManager; // エリアマネージャ
    protected Image bossHpGauge; // ボス用HPゲージ
    protected EnemyStatus enemyStatus;
    protected Enemy enemy;
    protected SpriteRenderer spriteRenderer;// 敵スプライト
    protected Rigidbody2D rb2D;

    // 各種変数
    // 基礎データ(インスペクタから入力)
    [Header("最大体力(初期体力)")]
    protected int health;
    [Header("接触時アクターへのダメージ")]
    public int touchDamage;
    [Header("ボス敵フラグ(ONでボス敵として扱う。１ステージに１体のみ)")]
    public bool isBoss;
    [Header("ボス用被撃破パーティクルPrefab")]
    public GameObject bossDefeatEffect;

    [SerializeField] private float knockbackGravityScale = 1.5f; // ノックバック時の重力倍率
    [SerializeField] private float normalGravityScale = 1.0f; // 通常時の重力倍率

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

    // 状態管理
    private bool isInvincible = false;
    private bool isKnockback = false; // ノックバック中フラグ

    // カラー定義
    private static readonly Color NormalColor = Color.white;
    private static readonly Color DamagedColor = new Color(1.0f, 0.1f, 0.1f);

    public void InitializeBossHpGauge(Image hpGauge)
    {
        if (isBoss && hpGauge != null)
        {
            bossHpGauge = hpGauge;
            bossHpGauge.fillAmount = 1.0f;
        }
    }

    private void Die()
    {
        Game2D.Instance.EnemyDefeated();
    }

    // 初期化関数(AreaManager.csから呼出)
    public void Init(AreaManager _areaManager)
    {
        // 参照取得
        areaManager = _areaManager;
        
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 自身の最初の子オブジェクトに EnemyStatus がアタッチされていると仮定
        enemyStatus = this.transform.GetChild(0).GetComponent<EnemyStatus>();
        enemy = this.transform.GetChild(0).GetComponent<Enemy>();

        // 変数初期化
        rb2D.freezeRotation = true;
        nowHP = health = enemyStatus.GetHp();
        if (transform.localScale.x > 0.0f)
            rightFacing = true;

        rb2D.gravityScale = normalGravityScale; // 通常時の重力設定

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
            bossHpGauge = areaManager.stageManager.bossHPGage;
            bossHpGauge.fillAmount = 1.0f;
            // BGM再生
            areaManager.stageManager.PlayBossBGM();
        }
    }

    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    public bool TakeDamage(int damage, Vector2 attackDirection)
    {
        if (isDead || isInvincible) return false;

        nowHP -= damage;
        UpdateHpGauge();

        if (nowHP <= 0)
        {
            HandleDeath();
        }
        else
        {
            PlayDamageEffect();
            ApplyKnockback(attackDirection); // ノックバック処理
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
    /// プレイヤーへの接触攻撃
    /// </summary>
    public void AttackPlayer(GameObject player)
    {
        if (isVanishing) return;

        var playerStatus = player.GetComponent<PlayerStatus>();
        if (playerStatus != null)
        {
            playerStatus.SetMinusHp(touchDamage);
        }
    }

    /// <summary>
    /// 向きを設定
    /// </summary>
    public void SetFacingDirection(bool isFacingRight)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (isFacingRight ? 1 : -1);
        transform.localScale = scale;
    }

    /// <summary>
    /// HPゲージを更新
    /// </summary>
    private void UpdateHpGauge()
    {
        if (bossHpGauge != null)
        {
            float healthRatio = (float)nowHP / health;
            bossHpGauge.DOFillAmount(healthRatio, 0.5f);
        }
    }

    /// <summary>
    /// 死亡時の処理
    /// </summary>
    private void HandleDeath()
    {
        isDead = true;
        rb2D.linearVelocity = Vector2.zero;
        rb2D.bodyType = RigidbodyType2D.Kinematic;

        if (isBoss && bossDefeatEffect != null)
        {
            Instantiate(bossDefeatEffect, transform.position, Quaternion.identity);
        }

        spriteRenderer.DOFade(0, 0.15f)
            .SetLoops(7, LoopType.Yoyo)
            .OnComplete(() => Destroy(gameObject));
    }

    /// <summary>
    /// 被ダメージ演出
    /// </summary>
    private void PlayDamageEffect()
    {
        if (damageTween != null) damageTween.Kill();
        spriteRenderer.color = COL_DAMAGED;
        damageTween = spriteRenderer.DOColor(COL_DEFAULT, 1.0f);
    }

    private void ApplyKnockback(Vector2 attackDirection)
    {
        if (rb2D != null)
        {
            isKnockback = true; // ノックバック中フラグをセット
            rb2D.gravityScale = knockbackGravityScale; // ノックバック中は重力を増加

            Vector2 knockback = new Vector2(
                KNOCKBACK_X * attackDirection.x,
                KNOCKBACK_Y
            );
            rb2D.linearVelocity = knockback;

            // 一定時間後に通常状態へ戻す
            Invoke(nameof(EndKnockback), 0.5f); // 0.5秒後にノックバック終了
        }
    }

    private void EndKnockback()
    {
        isKnockback = false; // ノックバック中フラグを解除
        rb2D.gravityScale = normalGravityScale; // 重力を通常に戻す
    }
}
