using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticItem : MonoBehaviour
{
    // ================================
    // �A�C�e���Ǘ�
    // ================================

    // --- �z�肷��g���� ---
    // �Q�[���J�n����ItemWarehouse��InitWarehouse�ŏ�����
    // �Q�[���N���A��AddItemNum�ŏ����A�C�e�������X�V
    // GetItemWarehouse�ŃA�C�e�������擾����
    // �����i������邩�m�F

    // �X�V���Ă������ǂ����i�A�C�e�������Z
    public static bool IsUpdate = false;
    
    // �A�C�e�����i�[�z��(�A�C�e���f�[�^����A�C�e���̎�ސ����擾)
    public static int[] ItemWarehouse = new int[(int)ItemData.Type.MAX_ITEM];

    // �q�ɂ̒��g���������ς݂��ǂ���
    // �Q�[���J�n���P��̂�
    public static bool IsInit = false;

    public static void InitWarehouse()
    {
        // �������ς݂Ȃ��΂�
        if (IsInit) return;

        // �z��̒��g���O�ŏ�����
        for(int i = 0;i<ItemWarehouse.Length;++i)
        {
            ItemWarehouse[i] = 0;
        }
        IsInit = true;
    }


    // �A�C�e�����i�[�z����擾
    public static int[] GetItemWarehouse()
    {
        return ItemWarehouse;
    }

    // �Q�[���Ŏ擾�����A�C�e�������Z
    // @param...item �Q�[���V�[���ŃA�C�e�����i�[���Ă���z��
    public static void AddItemNum(int[] item)
    {
        // �z�񐔕�
        for (int i = 0; i < ItemWarehouse.Length; ++i)
        {
            // �擾�����A�C�e����ۑ�
            ItemWarehouse[i] += item[i];
        }
    }

}
