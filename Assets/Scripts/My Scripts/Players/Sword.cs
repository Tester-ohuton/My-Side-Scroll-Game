using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    void Start()
    {
        RightSwing();
    }

    public void RightSwing()
    {
        transform.localScale = new Vector3(1, 1, 1); // �X�v���C�g��ʏ�̕����ɐݒ�
    }

    public void LeftSwing()
    {
        transform.localScale = new Vector3(-1, 1, 1); // �X�v���C�g�𔽓]������
    }
}