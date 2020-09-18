using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCanvas : MonoBehaviour
{
    public GameObject selectImage;

    public Text currentGoldText;

    public List<ShopItem> items;


    private Canvas _canvas;
    public Canvas _Canvas 
    {
        get
        {
            if(_canvas == null)
            {
                _canvas = GetComponent<Canvas>();
            }
            return _canvas;
        }
        private set
        {
            _canvas = value;
        }
    }

    private bool IsOpen 
    {
        get
        {
            return _Canvas.enabled;
        }
    }

    private int _itemIndex;
    private int ItemIndex
    {
        get
        {
            return _itemIndex;
        }
        set
        {
            _itemIndex = Mathf.Clamp(value, 0, 3);
        }
    }

    private void Awake()
    {
        ItemIndex = 0;
    }

    private void Update()
    {
        if(IsOpen == false) { return; }

        currentGoldText.text = "Gold: " + Inventory.Instance.CurrentGold.ToString() + "G";

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCanvas();
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            ItemIndex --;
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            ItemIndex ++;
        }

        selectImage.transform.position = items[ItemIndex].transform.position;
    }

    public void OpenCanvas()
    {
        _Canvas.enabled = true;
    }

    public void CloseCanvas()
    {
        _Canvas.enabled = false;
    }

    public void LinkingItems(List<int> itemID)
    {
        for(int i = 0; i < 4; i ++)
        {
            items[i].LinkingTextOfData(itemID[i]);
        }
    }

    public Item Purchase()
    {
        Item item = new Item(items[ItemIndex].Code);

        return item;
    }
}
