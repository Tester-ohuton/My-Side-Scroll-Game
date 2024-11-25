using UnityEngine;
using System.Collections;

public class PlayerController2D : MonoBehaviour
{
    public float speed;       // �ړ����x
    public float jumpForce;   // �W�����v��
    public LayerMask groundLayer; // �n�ʔ���p���C���[
    public float gravity;   //�d��

    [SerializeField] private float knockbackGravityScale = 1.5f; // �m�b�N�o�b�N���̏d�͔{��
    [SerializeField] private float normalGravityScale = 1.0f; // �ʏ펞�̏d�͔{��

    // Ray�̒����ƃI�t�Z�b�g
    [SerializeField] private float rayLength = 1f;
    [SerializeField] private float rayOffset = 0.1f;

    private Rigidbody2D rb2D;
    private Animator animator;
    private bool isGrounded;
    private float moveInput;

    private float leaveTime = 0.0f; // ���u����
    private const float LeaveThreshold = 5.0f; // ���u���[�V��������臒l

    // �萔��`
    private readonly Color COL_DEFAULT = new Color(1.0f, 1.0f, 1.0f, 1.0f);    // �ʏ펞�J���[
    private readonly Color COL_DAMAGED = new Color(1.0f, 0.1f, 0.1f, 1.0f);    // ��_���[�W���J���[
    private const float KNOCKBACK_X = 1.8f; // ��_���[�W���m�b�N�o�b�N��(x����)
    private const float KNOCKBACK_Y = 0.3f; // ��_���[�W���m�b�N�o�b�N��(y����)

    // ��ԊǗ�
    private bool isInvincible = false;
    private bool isKnockback = false; // �m�b�N�o�b�N���t���O

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb2D.gravityScale = normalGravityScale; // �ʏ펞�̏d�͐ݒ�
    }

    void Update()
    {
        // �������͂��擾
        moveInput = Input.GetAxis("Horizontal");

        // �A�j���[�V�����̐ݒ�
        animator.SetBool("isWalk", moveInput != 0);

        // ���u���ԏ���
        HandleLeaveMotion();

        // �ڒn����
        isGrounded = CheckGrounded();
        animator.SetBool("isJump", !isGrounded);

        // �W�����v����
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // �v���C���[�̌������X�V
        UpdatePlayerDirection();

        // �U������
        HandleAttack();
    }

    void FixedUpdate()
    {
        // �ړ�����
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
            transform.localScale = new Vector3(1, 1, 1); // �E����
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // ������
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
        // Raycast�Œn�ʂƂ̐ڐG�𔻒�
        var origin = new Vector2(transform.position.x, transform.position.y - rayOffset);
        var hit = Physics2D.Raycast(origin, Vector2.down, rayLength, groundLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        // Debug�p��Ray������
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawRay(new Vector2(transform.position.x, transform.position.y - rayOffset), Vector2.down * rayLength);
    }

    private void ApplyKnockback(Vector2 attackDirection)
    {
        if (rb2D != null)
        {
            isKnockback = true; // �m�b�N�o�b�N���t���O���Z�b�g
            rb2D.gravityScale = knockbackGravityScale; // �m�b�N�o�b�N���͏d�͂𑝉�

            Vector2 knockback = new Vector2(
                KNOCKBACK_X * attackDirection.x,
                KNOCKBACK_Y
            );
            rb2D.linearVelocity = knockback;

            // ��莞�Ԍ�ɒʏ��Ԃ֖߂�
            Invoke(nameof(EndKnockback), 0.5f); // 0.5�b��Ƀm�b�N�o�b�N�I��
        }
    }

    private void EndKnockback()
    {
        isKnockback = false; // �m�b�N�o�b�N���t���O������
        rb2D.gravityScale = normalGravityScale; // �d�͂�ʏ�ɖ߂�
    }
}
