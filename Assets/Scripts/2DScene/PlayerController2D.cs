using UnityEngine;
using System.Collections;

public class PlayerController2D : MonoBehaviour
{
    public float speed;       // 移動速度
    public float jumpForce;   // ジャンプ力
    public LayerMask groundLayer; // 地面判定用レイヤー
    public float gravity;   //重力

    [SerializeField] private float knockbackGravityScale = 1.5f; // ノックバック時の重力倍率
    [SerializeField] private float normalGravityScale = 1.0f; // 通常時の重力倍率

    // Rayの長さとオフセット
    [SerializeField] private float rayLength = 1f;
    [SerializeField] private float rayOffset = 0.1f;

    private Rigidbody2D rb2D;
    private Animator animator;
    private bool isGrounded;
    private float moveInput;

    private float leaveTime = 0.0f; // 放置時間
    private const float LeaveThreshold = 5.0f; // 放置モーション時間閾値

    // 定数定義
    private readonly Color COL_DEFAULT = new Color(1.0f, 1.0f, 1.0f, 1.0f);    // 通常時カラー
    private readonly Color COL_DAMAGED = new Color(1.0f, 0.1f, 0.1f, 1.0f);    // 被ダメージ時カラー
    private const float KNOCKBACK_X = 1.8f; // 被ダメージ時ノックバック力(x方向)
    private const float KNOCKBACK_Y = 0.3f; // 被ダメージ時ノックバック力(y方向)

    // 状態管理
    private bool isInvincible = false;
    private bool isKnockback = false; // ノックバック中フラグ

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb2D.gravityScale = normalGravityScale; // 通常時の重力設定
    }

    void Update()
    {
        // 水平入力を取得
        moveInput = Input.GetAxis("Horizontal");

        // アニメーションの設定
        animator.SetBool("isWalk", moveInput != 0);

        // 放置時間処理
        HandleLeaveMotion();

        // 接地判定
        isGrounded = CheckGrounded();
        animator.SetBool("isJump", !isGrounded);

        // ジャンプ処理
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // プレイヤーの向きを更新
        UpdatePlayerDirection();

        // 攻撃処理
        HandleAttack();
    }

    void FixedUpdate()
    {
        // 移動処理
        rb2D.linearVelocity = new Vector2(moveInput * speed, rb2D.linearVelocity.y);
    }

    private void Jump()
    {
        rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);
    }

    private void HandleLeaveMotion()
    {
        if (moveInput == 0 && isGrounded)
        {
            leaveTime += Time.deltaTime;

            if (leaveTime > LeaveThreshold)
            {
                animator.SetBool("isLeave", true);
            }
        }
        else
        {
            leaveTime = 0.0f;
            animator.SetBool("isLeave", false);
        }
    }

    private void UpdatePlayerDirection()
    {
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // 右向き
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // 左向き
        }
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            animator.SetTrigger("Attack");
        }
    }

    private bool CheckGrounded()
    {
        // Raycastで地面との接触を判定
        var origin = new Vector2(transform.position.x, transform.position.y - rayOffset);
        var hit = Physics2D.Raycast(origin, Vector2.down, rayLength, groundLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        // Debug用のRayを可視化
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawRay(new Vector2(transform.position.x, transform.position.y - rayOffset), Vector2.down * rayLength);
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
