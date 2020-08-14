using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer
{
    private static Dictionary<int, Item> items;
    private static Dictionary<int, Item> Items
    {
        get
        {
            if(items == null)
            {
                items = new Dictionary<int, Item>();
            }
            return items;
        }
    }

    public static void CreateItem()
    {
        //TODO: 데이터 테이블 파싱후 아이템을 생성해서 Items에 저장
    }

    public static Item GetItem(int itemCode)
    {
        return Items[itemCode];
    }
}
