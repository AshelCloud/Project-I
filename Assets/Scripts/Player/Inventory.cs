using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;
using UnityEngine.UI;

enum ITEM : int
{
    HELMET = 0,
    ARMOR,
    WEAPON,
    ACCESSORIES,
    POTION
};

public class Inventory : Singleton<Inventory>
{
    //인벤토리 메뉴 이미지
    [SerializeField]
    private GameObject image_SelectSlot;
    [SerializeField]
    private GameObject image_SelectList;
    [SerializeField]
    private GameObject image_SelectDetach;

    //장비 슬롯 이미지
    [SerializeField]
    private Image[] equipSlot;

    //아이템 설명
    [SerializeField]
    private Text explanation;

    private static List<Item> inventory = new List<Item>();

    private static float currentGold = 0;

    private int selectSocket = 0;
    private int selectList = 0;

    private bool enterInventory = false;

    private Player player = null;

    private GameObject itemList = null;

    private int index = 1;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        itemList = GameObject.Find("Content");

        //**TEST**
        PutItemInventory(new Item(2));
        PutItemInventory(new Item(7));
        PutItemInventory(new Item(12));
        PutItemInventory(new Item(19));
        PutItemInventory(new Item(26));

        //**TEST**

        RenderSlot();
        RenderItemList();
    }

    private void LateUpdate()
    {
        //gameObject.transform.GetChild(0).GetComponent<Text>().text = currentGold.ToString();
        if (!enterInventory)
        {
            SelectSlot();
        }

        else
        {
            SelectItem();
        }
    }

    //장비 슬롯 선택
    private void SelectSlot()
    {
        //***********************슬롯 이동***********************
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectSocket--;
            if (selectSocket < 0)
            {
                selectSocket = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectSocket++;
            if (selectSocket > equipSlot.Length - 1)
            {
                selectSocket = equipSlot.Length - 1;
            }
        }

        //선택 이미지 이동
        image_SelectSlot.transform.position = equipSlot[selectSocket].transform.position;

        //슬롯 선택
        if (Input.GetKeyDown(KeyCode.A))
        {
            enterInventory = true;

            image_SelectList.SetActive(true);
        }

    }

    //인벤토리 선택
    private void SelectItem()
    {
        //A: 선택
        if (Input.GetKeyDown(KeyCode.A))
        {
            if(player.ChangeEquipment(selectSocket, inventory[selectList]))
            {
                inventory.RemoveAt(selectList);
                selectList = 0;
            }


            //if(image_SelectDetach.activeSelf && selectList == 0)
            //{
            //    inventory.Add(player.ItemSocket[selectSocket]);
            //    player.ItemSocket[selectSocket] = new Item(player.ItemSocket[selectSocket].itemType);
            //}

            //else if (selectSocket < 4)
            //{
            //    //빈 슬롯에 장착
            //    if (player.ItemSocket[selectSocket] == null &&
            //        player.ChangeEquipment(selectSocket, inventory[selectList]))
            //    {
            //        inventory.RemoveAt(selectList);
            //        selectList = 0;
            //    }

            //    //현재 슬롯에 장착된 아이템과 교체
            //    else if (player.ItemSocket[selectSocket] != null &&
            //            player.ChangeEquipment(selectSocket, inventory[selectList]))
            //    {
            //        Item temp = player.ItemSocket[selectSocket];
            //        inventory[selectList] = temp;
            //    }
            //}

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

        //***********************인벤토리 이동***********************
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

            //Out of range 방지
            if (selectList >= itemList.transform.childCount - 1)
            {
                selectList = itemList.transform.childCount - 1;
            }
        }
        image_SelectList.transform.position = itemList.transform.GetChild(selectList + index).position;
        //***********************인벤토리 이동***********************


        //장비 해제 활성화시
        if (image_SelectDetach.activeSelf)
        {
            index = 0;

            if (selectList < 1)
            {
                explanation.text = "장비 해제";
            }

            else
            {
                explanation.text = inventory[selectList].itemExplanation;
            }
        }

        //장비 해제 비활성화시
        else
        {
            index = 1;
            explanation.text = inventory[selectList].itemExplanation;
        }
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
        for (int i = 1; i < itemList.transform.childCount; i++)
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
        foreach (string key in player.ItemSocket.Keys)
        {
            if (player.ItemSocket[key] != null)
            {
                switch (key)
                {
                    case "Helm":
                        equipSlot[(int)ITEM.HELMET].sprite = ((Item)player.ItemSocket["Helm"]).spriteImage;
                        break;

                    case "Armor":
                        equipSlot[(int)ITEM.ARMOR].sprite = ((Item)player.ItemSocket["Armor"]).spriteImage;
                        break;

                    case "Weapon":
                        equipSlot[(int)ITEM.WEAPON].sprite = ((Item)player.ItemSocket["Weapon"]).spriteImage;
                        break;

                    case "Accessories":
                        equipSlot[(int)ITEM.ACCESSORIES].sprite = ((Item)player.ItemSocket["Accessories"]).spriteImage;
                        break;


                    default:
                        break;
                }
            }

            if (((Stack<Item>)player.ItemSocket["Potion"]).Count != 0)
            {
                equipSlot[(int)ITEM.POTION].sprite = ((Stack<Item>)player.ItemSocket["Potion"]).Peek().spriteImage;
                equipSlot[(int)ITEM.POTION].transform.GetChild(0).GetComponent<Text>().text = ((Stack<Item>)player.ItemSocket["Potion"]).Count.ToString();
            }
        }
    }
}
