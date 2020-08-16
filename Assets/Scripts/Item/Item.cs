﻿using UnityEngine;

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
        SetData();

        spriteRenderer.sprite = ResourcesContainer.Load<Sprite>("Sprites/Item/" + itemType + "/" + graphicAssetsName);
        Log.Print("Create Item: " + itemName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Inventory.Instance.PutItemInventory(gameObject.GetComponent<Item>());
            Destroy(gameObject);
        }
    }

    private void SetData()
    {
        //ItemContainer부터 Item Data 불러오기
        ItemData datas = ItemContainer.GetItem(ID);

        itemName = datas.Name;
        variableName = datas.VariableName;
        itemType = datas.ItemType;
        offensePower = datas.OffensePower;
        hp = datas.HP;
        speed = datas.Speed;
        getPlace = datas.GetPlace;
        specialEffects = datas.SpecialEffects;
        graphicAssetsName = datas.GraphicAssetsName;
    }
}
