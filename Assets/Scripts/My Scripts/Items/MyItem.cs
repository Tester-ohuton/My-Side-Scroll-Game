using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MyItem : MonoBehaviour
{
    // ================================
    // �v���C���[�̏����A�C�e��
    // ================================


    const int Num = (int)ItemData.Type.MAX_ITEM;

    // �擾�A�C�e���i�[�p�z��
    int[] Storage = new int[Num];


    bool fadestart = false;

    // Start is called before the first frame update
    void Start()
    {
        // �z��̗v�f����������
        for(int i = 0;i<Storage.Length;++i)
        {
            // �S�ĂO
            Storage[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �A�C�e���ɐG���Ă��Ȃ��Ƃ����false
        // �X�V������_��
        StaticItem.IsUpdate = false;
        // ��
        //if (Storage[0] == 2)
        //{
        //    if (!fadestart)
        //    {
        //        fadestart = true;
        //        FadeManager.Instance.LoadScene("Result", 2.0f);
        //        Debug.Log("a");
        //        //fadestart = false;
        //    }
            
        //}
       
    }

    // �A�C�e���i�[
    // @param...type �i�[����A�C�e���̎��
    public void AddItem(ItemData.Type type)
    {
        // �����œn���ꂽ�A�C�e���̐��𑝂₷
        Storage[(int)type] += 1;
        Debug.Log(Storage[(int)type]);
    }

    // �X�g���[�W�̒��g���擾
    public int[] GetStorage()
    {
        return Storage;
    }
}
