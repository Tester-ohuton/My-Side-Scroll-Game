using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// �S�G�l�~�[���ʏ����N���X
/// </summary>
public class EnemyBase : MonoBehaviour
{
    // �I�u�W�F�N�g�E�R���|�[�l���g
    [HideInInspector] public AreaManager areaManager; // �G���A�}�l�[�W��
    protected Rigidbody rb; // Rigidbody/
    protected Transform actorTransform; // ��l��(�A�N�^�[)��Transform
    protected Image bossHPGage; // �{�X�pHP�Q�[�W
    protected EnemyStatus enemyStatus;

    // �e��ϐ�
    // ��b�f�[�^(�C���X�y�N�^�������)
    [Header("�ő�̗�(�����̗�)")]
    protected int maxHP;
    [Header("�ڐG���A�N�^�[�ւ̃_���[�W")]
    public int touchDamage;
    [Header("�{�X�G�t���O(ON�Ń{�X�G�Ƃ��Ĉ����B�P�X�e�[�W�ɂP�̂̂�)")]
    public bool isBoss;
    [Header("�{�X�p�팂�j�p�[�e�B�N��Prefab")]
    public GameObject bossDefeatParticlePrefab;
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

    // �������֐�(AreaManager.cs����ďo)
    public void Init(AreaManager _areaManager)
    {
        // �Q�Ǝ擾
        areaManager = _areaManager;
        actorTransform = areaManager.stageManager.player.transform;
        rb = GetComponent<Rigidbody>();

        enemyStatus = this.transform.GetChild(0).GetComponent<EnemyStatus>();

        // �ϐ�������
        rb.freezeRotation = true;
        nowHP = maxHP = enemyStatus.GetHp();
        if (transform.localScale.x > 0.0f)
            rightFacing = true;

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
            bossHPGage = areaManager.stageManager.bossHPGage;
            bossHPGage.fillAmount = 1.0f;
            // BGM�Đ�
            areaManager.stageManager.PlayBossBGM();
        }
    }

    /// <summary>
    /// �_���[�W���󂯂�ۂɌĂяo�����
    /// </summary>
    /// <param name="damage">�_���[�W��</param>
    /// <returns>�_���[�W�����t���O true�Ő���</returns>
    public bool Damaged(int damage)
    {
        // �_���[�W����
        nowHP -= damage;
        // (�{�X�p)HP�Q�[�W�̕\�����X�V����
        if (bossHPGage != null)
        {
            float hpRatio = (float)nowHP / maxHP;
            bossHPGage.DOFillAmount(hpRatio, 0.5f);
        }

        if (nowHP <= 0.0f)
        {// HP0�̏ꍇ
         // ��_���[�WTween������
            if (damageTween != null)
                damageTween.Kill();
            damageTween = null;

            // ���Œ��t���O���Z�b�g
            isVanishing = true;
            // ���Œ��͕������Z�Ȃ�
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
            
            // ���̑����j������
            if (isBoss)
            {// �{�X���j��
             // �{�X���j�p�[�e�B�N���𐶐�
                var obj = Instantiate(bossDefeatParticlePrefab);
                obj.transform.position = transform.position;
                // �Q�[���N���A����
                areaManager.stageManager.StageClear();
            }
            else
            {// �U�R���j��
            }
        }
        else
        {// �܂�HP���c���Ă���ꍇ
         // ��_���[�WTween������
            if (damageTween != null)
                damageTween.Kill();
            damageTween = null;
            // ��_���[�W���o�Đ�

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
    /// �A�N�^�[�ɐڐG�_���[�W��^���鏈��
    /// </summary>
    public void BodyAttack(GameObject actorObj)
    {
        // ���g�����Œ��Ȃ疳��
        if (isVanishing)
            return;

        // �A�N�^�[�̃R���|�[�l���g���擾
        PlayerStatus playerStatus = actorObj.GetComponent<PlayerStatus>();
        if (playerStatus == null)
            return;

        // �A�N�^�[�ɐڐG�_���[�W��^����
        playerStatus.SetMinusHp(touchDamage);
    }

    /// <summary>
    /// �I�u�W�F�N�g�̌��������E�Ō��肷��
    /// </summary>
    /// <param name="isRight">�E�����t���O</param>
    public void SetFacingRight(bool isRight)
    {
        // �L�����N�^�[�̌�������
        Vector2 lscale = gameObject.transform.localScale;

        if (!isRight)
        {// ������
            lscale.x *= -1;

            // �E�����t���Ooff
            rightFacing = false;
        }
        else
        {// �E����
            lscale.x *= 1;

            // �E�����t���Oon
            rightFacing = true;
        }
    }
}