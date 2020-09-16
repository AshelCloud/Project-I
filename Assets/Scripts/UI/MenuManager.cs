using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private Stack<GameObject> menu = new Stack<GameObject>();

    private void Awake()
    {
        menu.Push(ResourcesContainer.Load<GameObject>("Prefabs/UI/SelectMenu"));
        Instantiate(menu.Peek(), transform);
    }

    private void LateUpdate()
    {
    }

    public void MenuSelect(GameObject selectedMenu)
    {
        menu.Push(selectedMenu);
        Instantiate(menu.Peek(), transform);
        Destroy(transform.GetChild(1).gameObject);
    }
}
