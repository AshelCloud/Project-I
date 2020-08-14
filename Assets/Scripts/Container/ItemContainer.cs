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
        AssetBundle localAssetBundle = AssetBundleContainer.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "jsons"));


        //if (localAssetBundle == null)
        //{
        //    Log.PrintError("Failed to load AssetBundle!");
        //}

        //if (ID == 0)
        //{
        //    Log.PrintError("Failed to Player Data, ID is null or 0");
        //    return;
        //}

        TextAsset json = AssetBundleContainer.LoadAsset<TextAsset>("jsons", "Item_Table");

        //Json 파싱
        var itemDatas = JsonManager.LoadJson<Serialization<string, ItemData>>(json).ToDictionary();

        //ID 값으로 해당되는 Data 저장
        //ID는 각 몬스터 스크립트에서 할당

        foreach (var datas in itemDatas)
        {
            Items.Add(int.Parse(datas.Key) , datas.Value);

            Debug.Log(datas.Value.Name);
        }


    }

    public static ItemData GetItem(int itemCode)
    {
        return Items[itemCode];
    }
}
