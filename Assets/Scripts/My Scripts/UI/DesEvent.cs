using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesEvent : MonoBehaviour
{
    public GameObject Canvas;
    /// <summary>
    /// “®‰æ‚ð•\Ž¦‚·‚é
    /// </summary>
    public void ActiveVideoPlayer()
    {
        Canvas.SetActive(true);
    }

    /// <summary>
    /// “®‰æ‚ð•\Ž¦‚³‚¹‚È‚¢
    /// </summary>
    public void DeactiveVideoPlayer()
    {
        Canvas.SetActive(false);
    }
}
