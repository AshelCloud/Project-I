﻿using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropBundleData
{
    public string DropBundleName = null;
    public List<int> Quantity;
    public List<int> ItemID;
    public List<int> Percentage;
    public string ProbabilitySum = null;
}

public class DropBundle : MonoBehaviour
{ 
    [SerializeField]
    private int id = 1;
    public int ID { set { id = value; } }

    private string dropBundleName = null;
    private List<int> quantity;
    private List<int> itemID;
    private List<int> percentage;
    private string probabilitySum = null;

    private void Awake()
    {
        SetData();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            DroppingBundle();
            Destroy(gameObject);
        }
    }

    private void SetData()
    {
        DropBundleData bundleDatas = DropBundleContainer.GetBundleData(id);

        dropBundleName = bundleDatas.DropBundleName;
        quantity = bundleDatas.Quantity;
        itemID = bundleDatas.ItemID;
        percentage = bundleDatas.Percentage;
        probabilitySum = bundleDatas.ProbabilitySum;
    }

    private void DroppingBundle()
    {
        int probability = Random.Range(1, 100);
        int cost = 0;
        for (int i = 0; i < percentage.Count; i++)
        {
            if (itemID[i] == 21)
            {
                if (probability <= percentage[i])
                {
                    cost = quantity[i];
                    Log.Print("Get Gold: " + cost.ToString());
                    FindObjectOfType<Inventory>().GetGold(cost);
                    break;
                }

                else
                {
                    probability -= percentage[i];
                }
            }

            else
            {
                Debug.Log(itemID[i]);
                //Instantiate(ResourcesContainer.Load<Item>("Prefabs/Item/Item"));
                //FindObjectOfType<Inventory>().PutItemInventory(item);
            }
        }

 
    }
}


