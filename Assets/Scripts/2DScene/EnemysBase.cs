using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemysBase : MonoBehaviour
{
    public float moveSpeed = 2f;

    // �I�u�W�F�N�g�E�R���|�[�l���g
    [HideInInspector] public AreaManager areaManager; // �G���A�}�l�[�W��
    protected Image bossHpGauge; // �{�X�pHP�Q�[�W
    protected EnemyStatus enemyStatus;
    protected Enemy enemy;
    protected SpriteRenderer spriteRenderer;// �G�X�v���C�g
    protected Rigidbody2D rb2D;

    // �e��ϐ�
    // ��b�f�[�^(�C���X�y�N�^�������)
    [Header("�ő�̗�(�����̗�)")]
    protected int health;
    [Header("�ڐG���A�N�^�[�ւ̃_���[�W")]
    public int touchDamage;
    [Header("�{�X�G�t���O(ON�Ń{�X�G�Ƃ��Ĉ����B�P�X�e�[�W�ɂP�̂̂�)")]
    public bool isBoss;
    [Header("�{�X�p�팂�j�p�[�e�B�N��Prefab")]
    public GameObject bossDefeatEffect;

    [SerializeField] private float knockbackGravityScale = 1.5f; // �m�b�N�o�b�N���̏d�͔{��
    [SerializeField] private float normalGravityScale = 1.0f; // �ʏ펞�̏d�͔{��

    // ���̑��f�[�^
    [HideInInspector] public int nowHP; // �c��HP

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

    [HideInInspector] public bool isVanishing; // ���Œ��t���O true�ŏ��Œ��ł���
    [HideInInspector] public bool isInvis; // ���G���[�h
    [HideInInspector] public bool rightFacing; // �E�����t���O(false�ō�����)

    // DoTween�p
    private Tween damageTween;  // ��_���[�W�����oTween

    // �萔��`
    private readonly Color COL_DEFAULT = new Color(1.0f, 1.0f, 1.0f, 1.0f);    // �ʏ펞�J���[
    private readonly Color COL_DAMAGED = new Color(1.0f, 0.1f, 0.1f, 1.0f);    // ��_���[�W���J���[
    private const float KNOCKBACK_X = 1.8f; // ��_���[�W���m�b�N�o�b�N��(x����)
    private const float KNOCKBACK_Y = 0.3f; // ��_���[�W���m�b�N�o�b�N��(y����)

    // ��ԊǗ�
    private bool isInvincible = false;
    private bool isKnockback = false; // �m�b�N�o�b�N���t���O

    // �J���[��`
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

    // �������֐�(AreaManager.cs����ďo)
    public void Init(AreaManager _areaManager)
    {
        // �Q�Ǝ擾
        areaManager = _areaManager;
        
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // ���g�̍ŏ��̎q�I�u�W�F�N�g�� EnemyStatus ���A�^�b�`����Ă���Ɖ���
        enemyStatus = this.transform.GetChild(0).GetComponent<EnemyStatus>();
        enemy = this.transform.GetChild(0).GetComponent<Enemy>();

        // �ϐ�������
        rb2D.freezeRotation = true;
        nowHP = health = enemyStatus.GetHp();
        if (transform.localScale.x > 0.0f)
            rightFacing = true;

        rb2D.gravityScale = normalGravityScale; // �ʏ펞�̏d�͐ݒ�

        // �G���A���A�N�e�B�u�ɂȂ�܂ŉ������������ҋ@
        gameObject.SetActive(false);
    }

    /// <summary>
    /// ���̃����X�^�[�̋���G���A�ɃA�N�^�[���i���������̏���(�G���A�A�N�e�B�u��������)
    /// </summary>
    public virtual void OnAreaActivated()
    {
        // ���̃����X�^�[���A�N�e�B�u��
        gameObject.SetActive(true);

        // �{�X�G�p����
        if (isBoss)
        {
            // HP�Q�[�W�\��
            areaManager.stageManager.bossHPGage.transform.parent.gameObject.SetActive(true);
            bossHpGauge = areaManager.stageManager.bossHPGage;
            bossHpGauge.fillAmount = 1.0f;
            // BGM�Đ�
            areaManager.stageManager.PlayBossBGM();
        }
    }

    /// <summary>
    /// �_���[�W���󂯂鏈��
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
            ApplyKnockback(attackDirection); // �m�b�N�o�b�N����
        }
        return true;
    }

    /// <summary>
    /// �G�l�~�[�����ł���ۂɌĂяo�����
    /// </summary>
    private void Vanish()
    {
        // �I�u�W�F�N�g����
        Destroy(gameObject);
    }

    /// <summary>
    /// �v���C���[�ւ̐ڐG�U��
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
    /// ������ݒ�
    /// </summary>
    public void SetFacingDirection(bool isFacingRight)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (isFacingRight ? 1 : -1);
        transform.localScale = scale;
    }

    /// <summary>
    /// HP�Q�[�W���X�V
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
    /// ���S���̏���
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
    /// ��_���[�W���o
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
