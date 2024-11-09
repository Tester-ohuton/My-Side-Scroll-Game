using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBG : MonoBehaviour
{
    // ================================
    // �w�i�X�N���[��
    // ================================

    private const float speed = 1f;

    Image image;

    Player player;

    // �ߋ����W�i�[�p 
    private float latepos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Actor").GetComponent<Player>();
        image = GetComponent<Image>();
        image.material.mainTextureOffset =
            new Vector2(0, 0);
        latepos = player.transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // ���ݍ��W�Ɖߋ����W���������i��ł���
            if (player.transform.position.x != latepos)
            {
                // �I�t�Z�b�g�X�V
                image.material.mainTextureOffset +=
                    new Vector2(
                        player.GetMoveDirection().x *
                        Time.deltaTime * 0.01f, 0);
                // �ߋ����W�X�V
                latepos = player.transform.position.x;
            }
        }
    }
}
