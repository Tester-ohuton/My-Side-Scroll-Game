using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationManager : MonoBehaviour
{
    // ================================
    // �����i�Ǘ�
    // ================================
    // �����i�̎�ސ�
    const int Deco_NUM = (int)DecorationData.DecorationType.MAX_DECO;

    // �����i�i�[�p
    private GameObject[] Decoration = new GameObject[Deco_NUM];

    // �����i������Ă��邩�ǂ���(�Y�����Ŏ�ނ𔻒f)
    private bool[] IsCreate = new bool[Deco_NUM];

    // �����i�̏��擾�p
    DecorationInfo[] decoInfo = new DecorationInfo[Deco_NUM];

    // Start is called before the first frame update
    void Start()
    {
        // �ŏ��̂P�񂾂�static�z���false�ŏ�����
        if(!StaticDecoration.isInit)
        {
            // �ێ��p�̍쐬�t���O��������false
            // �ێ��p�̃X�e�[�^�X���Z�t���O������
            StaticDecoration.InitFlag();
            // �������I�������珉���������t���O�I��
            StaticDecoration.isInit = true;

            StaticItem.InitWarehouse();

            StaticEnemy.InitWarehouse();
        }

        // �����i������Ă��邩�ǂ���
        for(int i = 0;i<IsCreate.Length;++i)
        {
            // �ێ����Ă���쐬�t���O���擾
            IsCreate[i] = StaticDecoration.GetIsCreate()[i];

            // �쐬�ς݂̏ꍇ���\�[�X�t�H���_����v���n�u�ǂݍ���
            if(IsCreate[i])
            {
                // �����i�ǂݍ���
                LoadDeco(i);
                // �ݒu
                Instantiate(Decoration[i], new Vector3(0, 3, -10), Quaternion.identity);
            }
            else
            {
                // �쐬�ς݂łȂ����null
                Decoration[i] = null;
            }
        }


        Debug.Log("��" + StaticItem.GetItemWarehouse()[0]);
        Debug.Log("�z" + StaticItem.GetItemWarehouse()[1]);
    }

    // Update is called once per frame
    void Update()
    {
        // �܂���쐬
        // �쐬�ς݃t���O�I�t�̂Ƃ�
        if(IsCreate[0] && Decoration[0] == null)
        {
            // �v���n�u�ǂݍ���
            LoadDeco(0);
            // �ǂݍ��񂾃v���n�u�𐶐�
            CreateDeco(0);
            // �v���C���[�̃X�e�[�^�X�ɉ��Z����
            SetBufStatus();
        }

        if (IsCreate[1] && Decoration[1] == null)
        {
            LoadDeco(1);
            CreateDeco(1);
            SetBufStatus();
        }
    }

    // �v���n�u�ǂݍ��݊֐�
    void LoadDeco(int no)
    {
        // �ԍ����Ƃɓǂݍ��ރv���n�u�ύX
        switch (no)
        {
            case 0:
                Decoration[no] = (GameObject)Resources.Load("Prefab/makura");
                break;

            case 1:
                Decoration[no] = (GameObject)Resources.Load("Prefab/bed");
                break;
        }
    }

    public void SetIsCreate(int no, bool flg)
    {
        IsCreate[no] = flg;
    }

    void CreateDeco(int no)
    {
        // �쐬�t���O�I��
        // �����i������ꂽ��
        IsCreate[no] = true;
        // �쐬�t���O�ێ����e�X�V
        StaticDecoration.SetIsCreate(IsCreate);
        // �I�u�W�F�N�g�i�����i�j�쐬
        Instantiate(Decoration[no], new Vector3(0, 3, -10), Quaternion.identity);
        // �쐬���������i�̏��擾
        decoInfo[no] = Decoration[no].GetComponent<DecorationInfo>();
    }

    // �����i�ɉ����ăv���C���[�X�e�[�^�X��ݒ�
    // �v���C���[�̃X�e�[�^�X��ێ�����static�ȕϐ��֐ݒ�
    public void SetBufStatus()
    {
        for(int i = 0;i<IsCreate.Length;++i)
        {
            // ����Ă��Ȃ��A���łɃX�e�[�^�X���Z�ς݁A�����i���NULL�̏ꍇ�Ƃ΂�
            if (!IsCreate[i] || StaticDecoration.GetIsAdd()[i] || !decoInfo[i]) continue;

            StaticStatus.SetStatus(
                    decoInfo[i].decoData.GetbufHP(),
                    decoInfo[i].decoData.GetbufATK(),
                    decoInfo[i].decoData.GetbufDEF(),
                    decoInfo[i].decoData.GetbufLUCK());

            // ���Z�ς݃t���O�I��
            StaticDecoration.GetIsAdd()[i] = true;
            
        }
    }
}
