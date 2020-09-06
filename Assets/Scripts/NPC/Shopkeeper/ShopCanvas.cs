using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCanvas : MonoBehaviour
{
    public List<ShopItem> items;

    public void LinkingItems(List<int> itemID)
    {
        for(int i = 0; i < 4; i ++)
        {
            items[i].LinkingTextOfData(itemID[i]);
        }
    }
}
