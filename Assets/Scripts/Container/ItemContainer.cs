using System.Collections.Generic;
using UnityEngine;

public class ItemContainer
{
    private static Dictionary<int, ItemData> items;

    //정적 생성자를 통한 초기화
    static ItemContainer()
    {
        if (items == null)
        {
            items = new Dictionary<int, ItemData>();
        }
    }

    public static void CreateItem()
    {
        TextAsset json = AssetBundleContainer.LoadAsset<TextAsset>("jsons", "Item_Table");
        if(json != null)
        {
            Log.Print("Item AssetBundle load Succes");
        }

        else
        {
            Log.PrintError("Error: Failed to load Item AssetBundle");
        }

        var itemDatas = JsonManager.LoadJson<Serialization<string, ItemData>>(json).ToDictionary();
        if (json != null)
        {
            Log.Print("Item Json data load Succes");
        }

        else
        {
            Log.PrintError("Error: Failed to load Item Json da");
        }

        foreach (var datas in itemDatas)
        {
            items.Add(int.Parse(datas.Key) , datas.Value);
        }
    }

    public static ItemData GetItem(int itemCode)
    {
        return items[itemCode];
    }
}
