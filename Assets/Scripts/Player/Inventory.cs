using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    static int gold = 0;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        
        else if(item.Type == "Money")
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
}
