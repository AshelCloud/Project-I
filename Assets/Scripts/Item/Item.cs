using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Diagnostics.PerformanceData;

public class Item : MonoBehaviour
{
    [System.Serializable]
    private class ItemData
    {
        public string Name = null;
        public string VariableName = null;
        public string ItemType = null;
        public float OffensePower = 0f;
        public float HP = 0f;
        public float Speed = 0f;
        public string GetPlace = null;
        public string SpecialEffects = null;
        public string GraphicAssetsName = null;
    }

    private ItemData itemData;

    [SerializeField]
    private int ID = 1;

    private string itemName = null;
    private string variableName = null;
    private string itemType = null;
    private float offensePower = 0f;
    private float hp = 0f;
    private float speed = 0f;
    private string getPlace = null;
    private string specialEffects = null;
    private string graphicAssetsName = null;

    public string Name { get { return itemName; } }
    public string Type { get { return itemType; } }

    public SpriteRenderer spriteRenderer { get { return gameObject.GetComponent<SpriteRenderer>(); } }

    private void Awake()
    {
        LoadToJsonData(ID);
        SetData();
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Item/" + itemType + "/" + graphicAssetsName);
    }

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
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

        TextAsset json = localAssetBundle.LoadAsset<TextAsset>("Item_Table");

        //Json 파싱
        var itemDatas = JsonManager.LoadJson<Serialization<string, ItemData>>(json).ToDictionary();

        //ID 값으로 해당되는 Data 저장
        //ID는 각 몬스터 스크립트에서 할당
        itemData = itemDatas[ID.ToString()];
    }

    private void SetData()
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
}
