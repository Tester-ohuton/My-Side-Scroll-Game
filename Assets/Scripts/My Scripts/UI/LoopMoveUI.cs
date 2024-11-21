using UnityEngine;

public class LoopMoveUI : MonoBehaviour
{
    public RectTransform uiElement; // UI�v�f��RectTransform
    public float speed = 100f; // �ړ����x
    public float topLimit = 300f; // ���
    public float bottomLimit = -300f; // ����
    private Vector2 direction = Vector2.up; // �ړ�����

    void Update()
    {
        // UI�v�f���ړ�
        uiElement.anchoredPosition += direction * speed * Time.deltaTime; // ����ɓ��B����������𔽓]

        if (uiElement.anchoredPosition.y >= topLimit)
        {
            direction = Vector2.down;

        }

        // �����ɓ��B����������𔽓]
        if (uiElement.anchoredPosition.y <= bottomLimit)
        {
            direction = Vector2.up;
        }
    }
}