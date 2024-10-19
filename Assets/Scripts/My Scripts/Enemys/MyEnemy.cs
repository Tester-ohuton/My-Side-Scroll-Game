using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemy : MonoBehaviour
{
    const int Num = (int)EnemyData.EnemyType.MAX_ENEMY;

    // �擾�A�C�e���i�[�p�z��
    int[] Storage = new int[Num];

    bool fadestart = false;

    void Start()
    {
        // �z��̗v�f����������
        for (int i = 0; i < Storage.Length; ++i)
        {
            // �S�ĂO
            Storage[i] = 0;
        }
    }


    void Update()
    {
        // hp��0�łȂ��Ƃ��ɏ��false
        StaticEnemy.IsUpdate = false;

        /*
        // ��
        if (Storage[0] == 2)
        {
            if (!fadestart)
            {
                fadestart = true;
                FadeManager.Instance.LoadScene("Result", 2.0f);
                Debug.Log("a");
                //fadestart = false;
            }
        }
        */
    }

    // �G�L�����i�[
    // @param...type �i�[����G�L�����̎��
    public void AddEnemy(EnemyData.EnemyType type)
    {
        // �����œn���ꂽ�G�L�����̐��𑝂₷
        Storage[(int)type]++;
        Debug.Log($"Storage: {Storage[(int)type]}");
    }

    // �X�g���[�W�̒��g���擾
    public int[] GetStorage()
    {
        return Storage;
    }
}
