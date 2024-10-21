using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{
    // ================================
    // �G�̏����擾���ăX�e�[�^�X�ݒ�
    // ================================

    // �G�l�~�[�̏��擾�p
    EnemyInfo enemyinfo;

    // �G��MaxHP
    [SerializeField] private int maxHP;

    // �G�̌��݂�HP
    [SerializeField] private int curHP;

    // �G�̍U����
    [SerializeField] int ATK;

    [SerializeField] int DEF;

    // HP�\���pUI
    [SerializeField] private GameObject HPUI;

    // HP�\���p�X���C�_�[
    private Slider hpSlider;

    
    public void Init()
    {
        // �G�̃f�[�^����X�e�[�^�X��ǂݎ��ݒ�
        enemyinfo = GetComponent<EnemyInfo>();
        curHP = maxHP = enemyinfo.enemyData.GetHp();
        ATK = enemyinfo.enemyData.GetATK();
        DEF = enemyinfo.enemyData.GetDEF();
        HPUI.SetActive(true);
        hpSlider = HPUI.transform.Find("HPBar").GetComponent<Slider>();
        hpSlider.value = 1.0f;

    }

    // Update is called once per frame
    void Update()
    {
        // ���FZ�L�[��HP����
        if(Input.GetKeyUp(KeyCode.Z))
        {
            SetHp(20);            
        }
    }

    public void SetHp(int hp)
    {
        this.curHP -= hp;

        // HP�\���pUI�̃A�b�v�f�[�g
        UpdateHPValue();
    }

    public int GetHp()
    {
        return curHP;
    }

    public int GetMaxhp()
    {
        return maxHP;
    }

    public int GetATK()
    {
        return ATK;
    }

    public int GetDEF()
    {
        return DEF;
    }

    // ���񂾂�HPUI���\���ɂ���
    public void HideStatusUI()
    {
        HPUI.SetActive(false);
    }

    public void UpdateHPValue()
    {
        hpSlider.value = (float)GetHp() / (float)GetMaxhp();
    }  
}
