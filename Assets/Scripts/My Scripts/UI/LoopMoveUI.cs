using UnityEngine;

public class LoopMoveUI : MonoBehaviour
{
    public RectTransform uiElement; // UI—v‘f‚ÌRectTransform
    public float speed = 100f; // ˆÚ“®‘¬“x
    public float topLimit = 300f; // ãŒÀ
    public float bottomLimit = -300f; // ‰ºŒÀ
    private Vector2 direction = Vector2.up; // ˆÚ“®•ûŒü

    void Update()
    {
        // UI—v‘f‚ðˆÚ“®
        uiElement.anchoredPosition += direction * speed * Time.deltaTime; // ãŒÀ‚É“ž’B‚µ‚½‚ç•ûŒü‚ð”½“]

        if (uiElement.anchoredPosition.y >= topLimit)
        {
            direction = Vector2.down;

        }

        // ‰ºŒÀ‚É“ž’B‚µ‚½‚ç•ûŒü‚ð”½“]
        if (uiElement.anchoredPosition.y <= bottomLimit)
        {
            direction = Vector2.up;
        }
    }
}