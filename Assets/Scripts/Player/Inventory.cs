using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    private static List<Item> inventory = new List<Item>();

    private static float currentGold = 0;

    private bool inventoryOpen = false;

    private void Awake()
    {
    }

    private void LateUpdate()
    {
        gameObject.transform.GetChild(0).GetComponent<Text>().text = currentGold.ToString();
    }

    public void PutItemInventory(Item item)
    {
        if(item.Type == "Helm")
        {

        }        

        else if(item.Type == "Armor")
        {

        }        

        else if(item.Type == "Accessories")
        {

        }  
        
        else if(item.Type == "Weapon")
        {

        }

        else if(item.Type == "Material")
        {

        }

        else
        {
            Log.PrintError("Unkown type item!");
        }

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
