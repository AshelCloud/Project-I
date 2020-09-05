using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShopKeeper : MonoBehaviour
{
    [System.Serializable]
    private class ShopKeepperData
    {
        public string NpcName;
        public List<string> SaleItem;
        public List<int> ItemID;
        public List<float> Cost;
    }

    public string ID = "1";

    private ShopKeepperData Data { get; set; }

    public ShopCanvas shopCanvas;

    private void Awake()
    {

        var text = AssetBundleContainer.LoadAsset<TextAsset>("jsons", "ShopKeepperTable");
        if (text == null)
        {
            AssetBundle result = AssetBundleContainer.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "jsons"));

            text = result.LoadAsset<TextAsset>("ShopKeepperTable");
        }

        ItemContainer.CreateItem();


        Data = JsonManager.LoadJson<Serialization<string, ShopKeepperData>>(text).ToDictionary()[ID];
        
        shopCanvas.LinkingItems(Data.ItemID);
    }
}
