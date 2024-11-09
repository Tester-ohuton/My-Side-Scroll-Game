using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBG : MonoBehaviour
{
    // ================================
    // 背景スクロール
    // ================================

    private const float speed = 1f;

    Image image;

    Player player;

    // 過去座標格納用 
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
            // 現在座標と過去座標が違ったら進んでいる
            if (player.transform.position.x != latepos)
            {
                // オフセット更新
                image.material.mainTextureOffset +=
                    new Vector2(
                        player.GetMoveDirection().x *
                        Time.deltaTime * 0.01f, 0);
                // 過去座標更新
                latepos = player.transform.position.x;
            }
        }
    }
}
