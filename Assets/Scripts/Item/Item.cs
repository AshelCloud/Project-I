using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string Name = null;
    public string VariableName = null;
    public string ItemType = null;
    public float OffensePower = 0f;
    public float Defense = 0f;
    public float HP = 0f;
    public float Speed = 0f;
    public string GetPlace = null;
    public string SpecialEffects = null;
    public string GraphicAssetsName = null;
    public string Route = null;
    public float Cost = 0f;
    public string ItemExplanation = null;
}

public class Item : MonoBehaviour
{
    [SerializeField]
    private int ID = 1;

    public string itemName { get; private set; } = null;
    public string variableName { get; private set; } = null;
    public string itemType { get; private set; } = null;
    public float offensePower { get; private set; } = 0f;
    public float defense { get; private set; } = 0f;
    public float hp { get; private set; } = 0f;
    public float speed { get; private set; } = 0f;
    public string getPlace { get; private set; } = null;
    public string specialEffects { get; private set; } = null;
    public string graphicAssetsName { get; private set; } = null;
    public string route { get; private set; } = null;
    public float cost { get; private set; } = 0f;
    public string itemExplanation { get; private set; } = null;
    
    public Sprite spriteImage { get; private set; }

    public Item(string item_Type) 
    {
        itemType = item_Type;
    }

    public Item(int id)
    {
        ID = id;

        SetData();

        spriteImage = ResourcesContainer.Load<Sprite>("Sprites/" + route);
        Log.Print("Create Item: " + itemName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<Inventory>().PutItemInventory(gameObject.GetComponent<Item>());
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
        route = datas.Route;
        cost = datas.Cost;
        itemExplanation = datas.ItemExplanation;
    }
}
