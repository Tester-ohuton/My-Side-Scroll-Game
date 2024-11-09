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
        transform.localScale = new Vector3(1, 1, 1); // スプライトを通常の方向に設定
    }

    public void LeftSwing()
    {
        transform.localScale = new Vector3(-1, 1, 1); // スプライトを反転させる
    }
}