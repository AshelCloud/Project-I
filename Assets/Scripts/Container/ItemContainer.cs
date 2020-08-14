using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemContainer
{
    private static Dictionary<int, ItemData> items;
    private static Dictionary<int, ItemData> Items
    {
        get
        {
            if(items == null)
            {
                items = new Dictionary<int, ItemData>();
            }
            return items;
        }
    }

    public static void CreateItem()
    {
        TextAsset json = AssetBundleContainer.LoadAsset<TextAsset>("jsons", "Item_Table");

        var itemDatas = JsonManager.LoadJson<Serialization<string, ItemData>>(json).ToDictionary();

        foreach (var datas in itemDatas)
        {
            Items.Add(int.Parse(datas.Key) , datas.Value);
        }
    }

    public static ItemData GetItem(int itemCode)
    {
        return Items[itemCode];
    }
}
