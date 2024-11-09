using UnityEngine;

public class UISway : MonoBehaviour
{
    public float swayAmount = 10f;  // �㉺�ɗh����
    public float swaySpeed = 2f;    // �h���X�s�[�h

    private RectTransform rectTransform;
    private Vector2 originalPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;  // �����ʒu��ۑ�
    }

    void Update()
    {
        // UI��Y���W���㉺�ɗh�炷
        float newY = originalPosition.y + Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        rectTransform.anchoredPosition = new Vector2(originalPosition.x, newY);
    }
}
