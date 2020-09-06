using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ShopKeeper : MonoBehaviour
{
    [System.Serializable]
    private class ShopKeepperData
    {
        public string NPCName;
        public List<string> SaleItem;
        public List<int> ItemID;
        public List<float> Cost;
    }

    public string ID = "1";
    public Text nameText;

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

        //TODO: 본 게임에 들어갈때 삭제 할 코드
        ItemContainer.CreateItem();


        Data = JsonManager.LoadJson<Serialization<string, ShopKeepperData>>(text).ToDictionary()[ID];
        
        nameText.text = Data.NPCName;
        shopCanvas.LinkingItems(Data.ItemID);
    }
}
