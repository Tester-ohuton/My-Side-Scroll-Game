using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesEvent : MonoBehaviour
{
    public GameObject Canvas;
    /// <summary>
    /// 動画を表示する
    /// </summary>
    public void ActiveVideoPlayer()
    {
        Canvas.SetActive(true);
    }

    /// <summary>
    /// 動画を表示させない
    /// </summary>
    public void DeactiveVideoPlayer()
    {
        Canvas.SetActive(false);
    }
}
