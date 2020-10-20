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
        public string NPCName = null;
        public List<string> SaleItem = null;
        public List<float> Cost = null;
        public List<int> ItemID = null;
    }

    public string ID = "1";
    public Text nameText;

    private ShopKeepperData Data { get; set; }

    private void Awake()
    {
        var text = AssetBundleContainer.LoadAsset<TextAsset>("jsons", "ShopKeepperTable");
        if (text == null)
        {
            AssetBundle result = AssetBundleContainer.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "jsons"));

            text = result.LoadAsset<TextAsset>("ShopKeepperTable");
        }

        Data = JsonManager.LoadJson<Serialization<string, ShopKeepperData>>(text).ToDictionary()[ID];

        nameText.text = Data.NPCName;

        List<int> copyItemID = new List<int>(Data.ItemID);
        ShopCanvas.Instance.LinkingItems(copyItemID);

        ShopCanvas.Instance.CloseCanvas();
    }
}
