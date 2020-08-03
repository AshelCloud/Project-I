using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private static Inventory instance;

    public static Inventory Instance
    {
        get
        {
            if(instance == null)
            {
                var obj = FindObjectOfType<Inventory>();
                if(obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newInventory = new GameObject("Inventory").AddComponent<Inventory>();
                    instance = newInventory;
                }
            }
            return instance;
        }

        private set
        {
            instance = value;
        }
    }

    private List<Item> inventory;

    private static int currentGold = 0;

    private Text goldText = null;

    private void Awake()
    {
        var objs = FindObjectsOfType<Inventory>();
        if(objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        goldText = gameObject.GetComponentInChildren<Text>();
    }

    void Update()
    {
        goldText.text = currentGold.ToString();
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

        Log.Print("Get item: " + item.Name);
        inventory.Add(item);
    }

    public void GetGold(int deposit)
    {
        currentGold += deposit;
    }
}
