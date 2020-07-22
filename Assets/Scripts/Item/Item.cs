using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class Item : MonoBehaviour
{
    [System.Serializable]
    private class ItemData
    {
        public string Name = null;
        public string VariableName = null;
        public string ItemType = null;
        public string OffensePower = null;
        public float HP = 0f;
        public float Speed = 0f;
        public string GetPlace = null;
        public string SpecialEffects = null;
        public string GraphicAssetsName = null;
    }

    private ItemData itemData;

    protected string itemName = null;
    protected string variableName = null;
    protected string itemType = null;
    protected string offensePower = null;
    protected float hp = 0f;
    protected float speed = 0f;
    protected string getPlace = null;
    protected string specialEffects = null;
    protected string graphicAssetsName = null;

    protected int ID = 1;

    void Awake()
    {
        itemName = itemData.Name;
        variableName = itemData.VariableName;
        itemType = itemData.ItemType;
        offensePower = itemData.OffensePower;
        hp = itemData.HP;
        speed = itemData.Speed;
        getPlace = itemData.GetPlace;
        specialEffects = itemData.SpecialEffects;
        graphicAssetsName = itemData.GraphicAssetsName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadToJsonData(int ID)
    {
        //테이블 ID는 1부터 시작
        //ID가 기본값이면 에러로그 출력
        AssetBundle localAssetBundle = AssetBundleContainer.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "jsons"));

        if (localAssetBundle == null)
        {
            Log.PrintError("Failed to load AssetBundle!");
        }

        if (ID == 0)
        {
            Log.PrintError("Failed to Player Data, ID is null or 0");
            return;
        }

        TextAsset json = localAssetBundle.LoadAsset<TextAsset>("Characters_Table");

        //Json 파싱
        var itemDatas = JsonManager.LoadJson<Serialization<string, ItemData>>(json).ToDictionary();

        //ID 값으로 해당되는 Data 저장
        //ID는 각 몬스터 스크립트에서 할당
        itemData = itemDatas[ID.ToString()];
    }
}
