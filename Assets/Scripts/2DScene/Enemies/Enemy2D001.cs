using System.Collections;
using UnityEngine;

public class Enemy2D001 : EnemysBase
{
    public enum EnemyAiState
    {
        WAIT,           // 行動を一旦停止
        MOVE,           // 移動
        ATTACK,         // 停止して攻撃
        MOVEANDATTACK,  // 移動しながら攻撃
        IDLE,           // 待機
        AVOID,          // 回避
        DIE,        // 倒れる
        KNOCK,      // ノックバック
        PLAYER_DIE, // プレイヤーが倒れた後
    }

    [Header("AI設定")]
    public EnemyAiState aiState = EnemyAiState.WAIT;
    public float shootDistance = 5.0f; // 攻撃を開始する距離
    public Transform playerTransform; // プレイヤーのTransform（追跡対象）
    public GameObject playerObject;
    public bool enemyCanShoot = true; // 攻撃可能かどうか

    private EnemyAiState nextState;   // 次のAIステート
    private bool isAiStateRunning = false; // AIルーチン実行中フラグ
    private bool wait = false;        // 一時停止用
    private bool isChasing = false;   // 追跡中フラグ
    private float distance;           // プレイヤーとの距離

    private void Update()
    {
        // 体力が0以下でモード変更
        if (enemyStatus != null && enemyStatus.GetHp() <= 0)
        {
            nextState = EnemyAiState.DIE;
        }

        // プレイヤーが倒れたら歩きモードへ
        // 歩きモードになったら入らない
        if (playerObject != null && playerObject.GetComponent<PlayerStatus>().GetCurHp() <= 0 &&
            aiState != EnemyAiState.MOVE)
        {
            nextState = EnemyAiState.PLAYER_DIE;
        }

        if (playerTransform != null)
        {
            distance = Vector2.Distance(transform.position, playerTransform.position);
        }
        UpdateAI();
    }

    private void SetAi()
    {
        if (isAiStateRunning)
        {
            return;
        }

        InitAi();
        AiMainRoutine();

        aiState = nextState;

        StartCoroutine(AiTimer());
    }

    private void InitAi()
    {
        // 初期化処理が必要ならここに記述
        isAiStateRunning = false;
    }

    private void AiMainRoutine()
    {
        if (wait)
        {
            nextState = EnemyAiState.WAIT;
            wait = false;
            return;
        }

        if (enemyCanShoot && isChasing && distance < shootDistance)
        {
            nextState = EnemyAiState.MOVEANDATTACK;
        }
        else if (distance < shootDistance)
        {
            isChasing = true;
            nextState = EnemyAiState.MOVE;
        }
        else
        {
            isChasing = false;
            nextState = EnemyAiState.IDLE;
        }
    }

    private void UpdateAI()
    {
        SetAi();

        switch (aiState)
        {
            case EnemyAiState.WAIT:
                Wait();
                break;
            case EnemyAiState.MOVE:
                Move();
                break;
            case EnemyAiState.ATTACK:
                Attack();
                break;
            case EnemyAiState.MOVEANDATTACK:
                MoveAndAttack();
                break;
            case EnemyAiState.IDLE:
                Idle();
                break;
            case EnemyAiState.AVOID:
                Avoid();
                break;
            case EnemyAiState.DIE:
                Die();
                break;
            case EnemyAiState.PLAYER_DIE:
                PlayerDie();
                break;
            case EnemyAiState.KNOCK:
                Knock();
                break;
        }
    }

    private IEnumerator AiTimer()
    {
        isAiStateRunning = true;
        yield return new WaitForSeconds(1.0f); // 1秒待機（ステート間の切り替えタイミング調整）
        isAiStateRunning = false;
    }

    private void Wait()
    {
        // 待機処理
        //Debug.Log("AI: Wait");
    }

    private void Move()
    {
        // プレイヤーに向かって移動
        //Debug.Log("AI: Move");
        if (playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }
    }

    private void Attack()
    {
        // 攻撃処理
        //Debug.Log("AI: Attack");
        AttackPlayer(playerObject);
    }

    private void MoveAndAttack()
    {
        // 移動しながら攻撃
        //Debug.Log("AI: Move and Attack");
        Move();
        Attack();
    }

    private void Idle()
    {
        // 何もしない（Idle状態）
        //Debug.Log("AI: Idle");
    }

    private void Avoid()
    {
        // プレイヤーから離れる（回避）
        //Debug.Log("AI: Avoid");
        if (playerTransform != null)
        {
            Vector2 direction = (transform.position - playerTransform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }
    }

    private void Die()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (enemy != null)
            {
                enemy.SetIsDead(true);
            }

            if (!isDead)
            {
                StaticEnemy.IsUpdate = true;
                isDead = true;
            }
        }
    }

    private void PlayerDie()
    {
        nextState = EnemyAiState.IDLE;
    }

    private void Knock()
    {
        Vector2 attackDirection = (enemy.transform.position - playerObject.transform.position).normalized;
        TakeDamage(20, attackDirection);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb2D.bodyType = RigidbodyType2D.Kinematic;

        if (collision.gameObject.tag == "Player")
        {
            ////離れるモードへ
            nextState = EnemyAiState.AVOID;
            Debug.Log("ぶつかった");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        rb2D.bodyType = RigidbodyType2D.Dynamic;

        if (collision.gameObject.tag == "Player")
        {
            ////突進
            nextState = EnemyAiState.WAIT;
            Debug.Log("突進");
        }
    }
}
