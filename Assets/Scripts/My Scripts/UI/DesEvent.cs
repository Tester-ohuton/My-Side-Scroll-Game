using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesEvent : MonoBehaviour
{
    public GameObject Canvas;
    /// <summary>
    /// �����\������
    /// </summary>
    public void ActiveVideoPlayer()
    {
        Canvas.SetActive(true);
    }

    /// <summary>
    /// �����\�������Ȃ�
    /// </summary>
    public void DeactiveVideoPlayer()
    {
        Canvas.SetActive(false);
    }
}
