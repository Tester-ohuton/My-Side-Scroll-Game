using UnityEngine;

public class UISway : MonoBehaviour
{
    public float swayAmount = 10f;  // 上下に揺れる量
    public float swaySpeed = 2f;    // 揺れるスピード

    private RectTransform rectTransform;
    private Vector2 originalPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;  // 初期位置を保存
    }

    void Update()
    {
        // UIのY座標を上下に揺らす
        float newY = originalPosition.y + Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        rectTransform.anchoredPosition = new Vector2(originalPosition.x, newY);
    }
}
