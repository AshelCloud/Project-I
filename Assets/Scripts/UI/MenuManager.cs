using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private Stack<GameObject> menus = new Stack<GameObject>();
    private Player player = null;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        var selectMenu = Instantiate(ResourcesContainer.Load<GameObject>("Prefabs/UI/SelectMenu"), transform);
        menus.Push(selectMenu);
    }


    public void MenuSelect(GameObject selectedMenu)
    {
        menus.Push(Instantiate(selectedMenu, transform));
    }

    public void MenuExit()
    {
        Destroy(menus.Pop());
        if(menus.Count <= 0)
        {
            Destroy(this.gameObject);
            player.menuOpened = false;
        }
    }
}
