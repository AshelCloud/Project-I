using UnityEngine;
using UnityEngine.UI;

public class SelectMenu : MonoBehaviour
{
    [SerializeField]
    private Color highlightColor = Color.white;

    [SerializeField]
    private Image[] menuIcons;

    [SerializeField]
    private GameObject[] menus;

    private MenuManager pauseMenu = null;

    private int selectIndex = 0;
    private int menuCount = 0;
    

    private void Awake()
    {
        menuCount = menuIcons.Length;
        menuIcons[selectIndex].color = highlightColor;

        pauseMenu = GameObject.Find("PauseMenu(Clone)").GetComponent<MenuManager>();
    }

    private void LateUpdate()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            menuIcons[selectIndex].color = new Color(255, 255, 255);
            selectIndex--;

            if (selectIndex < 0)
            {
                selectIndex = 0;
                menuIcons[selectIndex].color = highlightColor;
            }

            else
            {
                menuIcons[selectIndex].color = highlightColor;
            }
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            menuIcons[selectIndex].color = new Color(255, 255, 255);
            selectIndex++;

            if (selectIndex > menuCount - 1)
            {
                selectIndex = menuCount - 1;
                menuIcons[selectIndex].color = highlightColor;
            }

            else
            {
                menuIcons[selectIndex].color = highlightColor;
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            pauseMenu.MenuSelect(menus[selectIndex]);
        }
    }
}
