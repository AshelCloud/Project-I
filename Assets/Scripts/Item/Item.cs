using UnityEngine;

//나중에 접근 제한이 필요할 수도?
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

    public string itemName { get; private set; }
    private string variableName = null;
    private string itemType = null;
    private float offensePower = 0f;
    private float defense = 0f;
    private float hp = 0f;
    private float speed = 0f;
    private string getPlace = null;
    private string specialEffects = null;
    private string graphicAssetsName = null;
    public string route { get; private set; }
    public float cost { get; private set; }
    private string itemExplanation = null;

    public string Type { get { return itemType; } }

    public Sprite sprite { get; private set; }

    public Item(int id)
    {
        ID = id;

        SetData();
        Debug.Log("Sprites/" + route);

        sprite = ResourcesContainer.Load<Sprite>("Sprites/" + route);
        Debug.Log(sprite);
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
