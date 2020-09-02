using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup group_1, group_2, group_3 = null;

    private void Start()
    {
        group_1.alpha = 1f;
        group_2.alpha = 0f;
        group_3.alpha = 0f;
    }

    public void SelectStatisticsMenu()
    {
        group_1.alpha = 1f;
        group_2.alpha = 0f;
        group_3.alpha = 0f;
    }

    public void SelectInventoryMenu()
    {
        group_1.alpha = 0f;
        group_2.alpha = 1f;
        group_3.alpha = 0f;
    }

    public void SelectSettingMenu()
    {
        group_1.alpha = 0f;
        group_2.alpha = 0f;
        group_3.alpha = 1f;
    }
}
