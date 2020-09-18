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
        public List<float> Cost;
        public List<int> ItemID;
    }

    public string ID = "1";
    public Text nameText;

    private ShopKeepperData Data { get; set; }

    public ShopCanvas _ShopCanvas { get; private set; }

    private void Awake()
    {
        var text = AssetBundleContainer.LoadAsset<TextAsset>("jsons", "ShopKeepperTable");
        if (text == null)
        {
            AssetBundle result = AssetBundleContainer.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "jsons"));

            text = result.LoadAsset<TextAsset>("ShopKeepperTable");
        }

        //TODO: 본 게임에 들어갈때 삭제 할 코드
        //ItemContainer.CreateItem();

        Data = JsonManager.LoadJson<Serialization<string, ShopKeepperData>>(text).ToDictionary()[ID];

        _ShopCanvas = GetComponentInChildren<ShopCanvas>();

        nameText.text = Data.NPCName;

        List<int> copyItemID = new List<int>(Data.ItemID);
        _ShopCanvas.LinkingItems(copyItemID);

        _ShopCanvas.CloseCanvas();
    }

    public void ShopOpen()
    {
        _ShopCanvas.OpenCanvas();
    }
}
