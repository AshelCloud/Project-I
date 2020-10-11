using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum ITEM : int
{
    HELMET = 0,
    ARMOR,
    ACCESSORIES,
    WEAPON,
    POTION
};

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject image_SelectEquip;

    [SerializeField]
    private GameObject image_SelectList;


    [SerializeField]
    private Image[] equipSlot;

    [SerializeField]
    private Text explanation;

    private static List<Item> inventory = new List<Item>();

    private static float currentGold = 0;

    private int selectEquip = 0;
    private int selectList = 0;

    private string selectSlotType = null;

    private bool enterInventory = false;


    private Player player = null;

    private GameObject itemList = null;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        itemList = GameObject.Find("Content");

        //**TEST**
        //PutItemInventory(new Item(1));
        PutItemInventory(new Item(2));
        PutItemInventory(new Item(7));
        PutItemInventory(new Item(12));
        PutItemInventory(new Item(19));
        PutItemInventory(new Item(26));

        player.consumSocket.Push(new Item(26));
        //**TEST**

        RenderSlot();
        RenderItemList();
    }

    private void LateUpdate()
    {
        //gameObject.transform.GetChild(0).GetComponent<Text>().text = currentGold.ToString();
        if (!enterInventory)
        {
            SelectEquipment();
        }

        else
        {
            SelectItem();
        }
    }

    private void SelectEquipment()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectEquip--;
            if (selectEquip < 0)
            {
                selectEquip = 0;
            }

        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectEquip++;
            if (selectEquip > equipSlot.Length - 1)
            {
                selectEquip = equipSlot.Length - 1;
            }
        }

        image_SelectEquip.transform.position = equipSlot[selectEquip].transform.position;

        if (Input.GetKeyDown(KeyCode.A))
        {
            enterInventory = true;

            image_SelectList.SetActive(true);
        }

        if (selectEquip != equipSlot.Length - 1)
        {
            explanation.text = player.itemSocket[selectEquip].itemExplanation;
        }

        else
        {
            explanation.text = player.consumSocket.Peek().itemExplanation;
        }
    }

    private void SelectItem()
    {
        //A: 선택
        if (Input.GetKeyDown(KeyCode.A))
        {
            //빈 슬롯에 장착
            if (player.itemSocket[selectEquip].itemName == null)
            {
                player.ChangeEquipment(selectEquip, inventory[selectList]);
                inventory.RemoveAt(selectList);
            }

            //현재 슬롯에 장착된 아이템과 교체
            else
            {
                Item temp = player.itemSocket[selectEquip];
                player.ChangeEquipment(selectEquip, inventory[selectList]);
                inventory[selectList] = temp;
            }

            //변경사항 렌더링
            ClearItemList();
            RenderItemList();
            RenderSlot();
        }

        //S: 취소
        if (Input.GetKeyDown(KeyCode.S))
        {
            image_SelectList.SetActive(false);
            selectList = 0;
            enterInventory = false;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectList--;
            if (selectList < 0)
            {
                selectList = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectList++;
            if (selectList > itemList.transform.childCount - 1)
            {
                selectList = itemList.transform.childCount - 1;
            }
        }

        image_SelectList.transform.position = itemList.transform.GetChild(selectList).position;
        explanation.text = inventory[selectList].itemExplanation;
    }



    public void GetGold(int deposit)
    {
        currentGold += deposit;
    }

    public void PerchasingItem(Item item)
    {
        currentGold -= item.cost;
        inventory.Add(item);

        var slot = Instantiate(ResourcesContainer.Load<GameObject>("Prefabs/UI/Slot"), GameObject.Find("Content").transform);
        slot.GetComponent<Slot>().SetField(item.spriteImage, item.itemName);
        slot.transform.SetParent(GameObject.Find("Content").transform, false);
    }

    public void PutItemInventory(Item item)
    {
        Log.Print("Get item: " + item.itemName);
        inventory.Add(item);
    }

    private void ClearItemList()
    {
        for (int i = 0; i < itemList.transform.childCount; i++)
        {
            Destroy(itemList.transform.GetChild(i).gameObject);
        }
    }

    private void RenderItemList()
    {
        foreach (var item in inventory)
        {
            var slot = Instantiate(ResourcesContainer.Load<GameObject>("Prefabs/UI/Slot"), GameObject.Find("Content").transform);
            slot.GetComponent<Slot>().SetField(item.spriteImage, item.itemName);
            slot.transform.SetParent(GameObject.Find("Content").transform, false);
        }
    }

    private void RenderSlot()
    {
        foreach (Item sockets in player.itemSocket)
        {
            switch (sockets.itemType)
            {
                case "Helm":
                    equipSlot[(int)ITEM.HELMET].sprite = player.itemSocket[0].spriteImage;
                    break;

                case "Armor":
                    equipSlot[(int)ITEM.ARMOR].sprite = player.itemSocket[(int)ITEM.ARMOR].spriteImage;
                    break;

                case "Accessories":
                    equipSlot[(int)ITEM.ACCESSORIES].sprite = player.itemSocket[(int)ITEM.ACCESSORIES].spriteImage;
                    break;

                case "Weapon":
                    equipSlot[(int)ITEM.WEAPON].sprite = player.itemSocket[(int)ITEM.WEAPON].spriteImage;
                    break;

                default:
                    break;
            }
        }

        equipSlot[(int)ITEM.POTION].sprite = player.consumSocket.Peek().spriteImage;
    }
}
