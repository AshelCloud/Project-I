using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private static List<Item> inventory = new List<Item>();

    private static float currentGold = 0;

    [SerializeField]
    private GameObject selectEquipSlot;

    [SerializeField]
    private Image[] equipSlot;

    private int selectIndex = 0;

    private bool enterInventory = false;

    private void Awake()
    {
    }

    private void LateUpdate()
    {
        gameObject.transform.GetChild(0).GetComponent<Text>().text = currentGold.ToString();
        if(!enterInventory)
        {
            SelectEquipment();
        }
        else
        {
            SelectInventory();
        }
    }

    private void SelectEquipment()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectIndex--;
            if (selectIndex < 0)
            {
                selectIndex = 0;
            }

        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectIndex++;
            if (selectIndex > equipSlot.Length - 1)
            {
                selectIndex = equipSlot.Length - 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            enterInventory = true;
        }

        selectEquipSlot.transform.position = equipSlot[selectIndex].transform.position;
    }

    private void SelectInventory()
    {

    }
    public void PutItemInventory(Item item)
    {
        Log.Print("Get item: " + item.itemName);
        inventory.Add(item);

        var index = Instantiate(ResourcesContainer.Load<GameObject>("Prefabs/UI/Slot"), GameObject.Find("Content").transform);
        index.GetComponent<Slot>().SetField(item.sprite, item.itemName);
        index.transform.SetParent(GameObject.Find("Content").transform, false);
    }

    public void GetGold(int deposit)
    {
        currentGold += deposit;
    }

    public void PerchasingItem(Item item)
    {
        currentGold -= item.cost;
        inventory.Add(item);

        var slot = Instantiate(ResourcesContainer.Load<GameObject>("Prefabs/UI/Slot"), GameObject.Find("Content").transform);
        slot.GetComponent<Slot>().SetField(item.sprite, item.itemName);
        slot.transform.SetParent(GameObject.Find("Content").transform, false);
    }
}
