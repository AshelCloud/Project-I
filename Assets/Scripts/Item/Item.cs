using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Diagnostics.PerformanceData;
using UnityEngine.Experimental.Audio.Google;

//나중에 접근 제한 하던지 말던지
[System.Serializable]
public class ItemData
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

public class Item : MonoBehaviour
{

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GetItem();
            Destroy(gameObject);
        }
    }

    //TODO: ItemContainer로 이주
    private void LoadToJsonData(int ID)
    {

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

    private void GetItem()
    {
        Inventory.Instance.PutItemInventory(gameObject.GetComponent<Item>());
    }
}
