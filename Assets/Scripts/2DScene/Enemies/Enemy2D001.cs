using System.Collections;
using UnityEngine;

public class Enemy2D001 : EnemysBase
{
    public enum EnemyAiState
    {
        WAIT,           // �s������U��~
        MOVE,           // �ړ�
        ATTACK,         // ��~���čU��
        MOVEANDATTACK,  // �ړ����Ȃ���U��
        IDLE,           // �ҋ@
        AVOID,          // ���
        DIE,        // �|���
        KNOCK,      // �m�b�N�o�b�N
        PLAYER_DIE, // �v���C���[���|�ꂽ��
    }

    [Header("AI�ݒ�")]
    public EnemyAiState aiState = EnemyAiState.WAIT;
    public float shootDistance = 5.0f; // �U�����J�n���鋗��
    public Transform playerTransform; // �v���C���[��Transform�i�ǐՑΏہj
    public GameObject playerObject;
    public bool enemyCanShoot = true; // �U���\���ǂ���

    private EnemyAiState nextState;   // ����AI�X�e�[�g
    private bool isAiStateRunning = false; // AI���[�`�����s���t���O
    private bool wait = false;        // �ꎞ��~�p
    private bool isChasing = false;   // �ǐՒ��t���O
    private float distance;           // �v���C���[�Ƃ̋���

    private void Update()
    {
        // �̗͂�0�ȉ��Ń��[�h�ύX
        if (enemyStatus != null && enemyStatus.GetHp() <= 0)
        {
            nextState = EnemyAiState.DIE;
        }

        // �v���C���[���|�ꂽ��������[�h��
        // �������[�h�ɂȂ��������Ȃ�
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
        // �������������K�v�Ȃ炱���ɋL�q
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
        yield return new WaitForSeconds(1.0f); // 1�b�ҋ@�i�X�e�[�g�Ԃ̐؂�ւ��^�C�~���O�����j
        isAiStateRunning = false;
    }

    private void Wait()
    {
        // �ҋ@����
        //Debug.Log("AI: Wait");
    }

    private void Move()
    {
        // �v���C���[�Ɍ������Ĉړ�
        //Debug.Log("AI: Move");
        if (playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }
    }

    private void Attack()
    {
        // �U������
        //Debug.Log("AI: Attack");
        AttackPlayer(playerObject);
    }

    private void MoveAndAttack()
    {
        // �ړ����Ȃ���U��
        //Debug.Log("AI: Move and Attack");
        Move();
        Attack();
    }

    private void Idle()
    {
        // �������Ȃ��iIdle��ԁj
        //Debug.Log("AI: Idle");
    }

    private void Avoid()
    {
        // �v���C���[���痣���i����j
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
            ////����郂�[�h��
            nextState = EnemyAiState.AVOID;
            Debug.Log("�Ԃ�����");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        rb2D.bodyType = RigidbodyType2D.Dynamic;

        if (collision.gameObject.tag == "Player")
        {
            ////�ːi
            nextState = EnemyAiState.WAIT;
            Debug.Log("�ːi");
        }
    }
}
