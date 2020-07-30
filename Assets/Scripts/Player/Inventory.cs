using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection.Emit;
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
        Log.Print("Get item: " + item.Name);
        inventory.Add(item);
    }
}
