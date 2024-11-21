using UnityEngine;
using UnityEngine.UI;

public class ChoiceLevelNo : MonoBehaviour
{
    [SerializeField] private Button[] levelButtons;

    public void LevelNo(int no)
    {
        StaticClear.StageNo = no;
    }

    private void Start()
    {
        for (int i = 1; i < levelButtons.Length; i++)
        {
            if (i < StaticClear.GetClearNum())
                levelButtons[i].interactable = true;
            else
                levelButtons[i].interactable = false;
        }
    }
}
