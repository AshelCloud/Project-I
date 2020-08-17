using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class DropBundleContainer
{
    private static Dictionary<int, DropBundleData> bundleCtnr;

    //정적 생성자를 통한 초기화
    static DropBundleContainer()
    {
        if (bundleCtnr == null)
        {
            bundleCtnr = new Dictionary<int, DropBundleData>();
        }
    }

    public static void LoadBundleData()
    {
        TextAsset json = AssetBundleContainer.LoadAsset<TextAsset>("jsons", "Item_Bundle_Table");
        if (json != null)
        {
            Log.Print("Drop table AssetBundle load Succes");
        }

        else
        {
            Log.PrintError("Error: Failed to load Drop table AssetBundle");
        }

        var bundleDatas = JsonManager.LoadJson<Serialization<string, DropBundleData>>(json).ToDictionary();
        if (json != null)
        {
            Log.Print("Drop table Json data load Succes");
        }

        else
        {
            Log.PrintError("Error: Failed to load Drop table Json data");
        }

        foreach (var datas in bundleDatas)
        {
            bundleCtnr.Add(int.Parse(datas.Key), datas.Value);
        }
    }

    public static DropBundleData GetBundleData(int bundleCode)
    {
        return bundleCtnr[bundleCode];
    }
}
