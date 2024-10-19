using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSelect : MonoBehaviour
{
    // ================================
    // ��鑕���i��I��
    // ================================


    GameObject decoMgr;

    // Start is called before the first frame update
    void Start()
    {
        decoMgr = GameObject.Find("DecorationMgr");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            MakeSelectDeco(DecorationData.DecorationType.MAKURA);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            MakeSelectDeco(DecorationData.DecorationType.BED);
        }
    }

    // �쐬���鑕���i��I��
    public void MakeSelectDeco(DecorationData.DecorationType type)
    {
        switch(type)
        {
            // �쐬����������
            case DecorationData.DecorationType.MAKURA:
                decoMgr.GetComponent<DecorationManager>().SetIsCreate((int)type, true);
                break;

            case DecorationData.DecorationType.BED:
                decoMgr.GetComponent<DecorationManager>().SetIsCreate((int)type, true);
                break;
        }
    }
}
