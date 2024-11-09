using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ItemNum : MonoBehaviour
{
    // ================================
    // �����A�C�e���\��UI
    // ================================

    public GameObject[] DispNum = null;

    private MyItem myitem;

    private Text[] itemNum = new Text[(int)ItemData.Type.MAX_ITEM];

    // Start is called before the first frame update
    void Start()
    {
        myitem = GameObject.Find("Actor").GetComponent<MyItem>();

        // �I�u�W�F�N�g����Text�R���|�[�l���g���擾
        itemNum[0] = DispNum[0].GetComponent<Text>();
        itemNum[1] = DispNum[1].GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // �e�L�X�g�̕\�������ւ���
        itemNum[0].text = "�ȁF" +  myitem.GetStorage()[0] + "/4";
        itemNum[1].text = "�z�F" + myitem.GetStorage()[1] + "/2";
    }
}
