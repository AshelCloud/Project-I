using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    private Stack<GameObject> menus;

    private Canvas _menuCanvas;
    public Canvas MenuCanvas
    {
        get
        {
            if (_menuCanvas == null)
            {
                _menuCanvas = GetComponent<Canvas>();
            }
            return _menuCanvas;
        }
    }

    private void Awake()
    {
        menus = new Stack<GameObject>();
    }

    private void Start()
    {
        var selectMenu = GetComponentInChildren<SelectMenu>().gameObject;

        menus.Push(selectMenu);
    }


    public void MenuSelect(GameObject selectedMenu)
    {
        menus.Push(Instantiate(selectedMenu, transform));
    }

    public void MenuExit()
    {
        Destroy(menus.Pop());
    }
}
