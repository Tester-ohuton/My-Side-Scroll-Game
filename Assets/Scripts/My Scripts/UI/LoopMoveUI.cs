using UnityEngine;

public class LoopMoveUI : MonoBehaviour
{
    public RectTransform uiElement; // UI要素のRectTransform
    public float speed = 100f; // 移動速度
    public float topLimit = 300f; // 上限
    public float bottomLimit = -300f; // 下限
    private Vector2 direction = Vector2.up; // 移動方向

    void Update()
    {
        // UI要素を移動
        uiElement.anchoredPosition += direction * speed * Time.deltaTime; // 上限に到達したら方向を反転

        if (uiElement.anchoredPosition.y >= topLimit)
        {
            direction = Vector2.down;

        }

        // 下限に到達したら方向を反転
        if (uiElement.anchoredPosition.y <= bottomLimit)
        {
            direction = Vector2.up;
        }
    }
}