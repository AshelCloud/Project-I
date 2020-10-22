using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public partial class Player
{
    //기본값 = 1
    private const int ID = 1;


    private PlayerData playerData;

    [Header("플레이어 능력치")]
    [SerializeField]
    private float offensePower = 0;
    [SerializeField]
    private float speed = 0f;    //이동 속도
    [SerializeField]
    private float jumpForce = 0f;    //점프력
    [SerializeField]
    private float rollForce = 0f;   //구르기 거리

    public float OffensePower { get { return offensePower; } }
    public float Defense { get; private set; }
    public float Speed { get { return speed; } }
    public float JumpForce { get { return jumpForce; } }
    public float RollForce { get { return rollForce; } }

    public const int consumableLimit = 5;

    public Stack<Item> ConsumSocket { get; private set; }

    public Hashtable ItemSocket { get; private set; }

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
        var playerDatas = JsonManager.LoadJson<Serialization<string, PlayerData>>(json).ToDictionary();

        //ID 값으로 해당되는 Data 저장
        //ID는 각 몬스터 스크립트에서 할당
        playerData = playerDatas[ID.ToString()];
    }

    private void InitData()
    {
        offensePower = playerData.Offensepower;
        Defense = playerData.Defense;
        HP = playerData.HP;
        MaxHP = playerData.HP;
        speed = playerData.Speed;
    }

    public bool ChangeEquipment(int socket, Item item)
    {
        switch (socket)
        {
            case (int)ITEM.HELMET:
                if(item.itemType == "Helm")
                {
                    ItemSocket["Helm"] = item;
                    UpdateStat(item);
                    return true;
                }

                else
                {
                    return false;
                }
            case (int)ITEM.ARMOR:
                if (item.itemType == "Armor")
                {
                    ItemSocket["Armor"] = item;
                    UpdateStat(item);
                    return true;
                }

                else
                {
                    return false;
                }

            case (int)ITEM.WEAPON:
                if (item.itemType == "Weapon")
                {
                    ItemSocket["Weapon"] = item;
                    UpdateStat(item);
                    return true;
                }

                else
                {
                    return false;
                }

            case (int)ITEM.ACCESSORIES:
                if (item.itemType == "Accessories")
                {
                    ItemSocket["Accessories"] = item;
                    UpdateStat(item);
                    return true;
                }

                else
                {
                    return false;
                }

            case (int)ITEM.POTION:
                if (item.itemType == "Potion")
                {
                    ((Stack<Item>)ItemSocket["Potion"]).Push(item);
                    return true;
                }

                else
                {
                    return false;
                }

            default:
                return false;
        }
    }
    private void UpdateStat(Item item)
    {
        offensePower = playerData.Offensepower + item.offensePower;
        Defense = playerData.Defense = item.defense;
        MaxHP = playerData.HP + item.hp;
        speed = playerData.Speed + item.speed;
    }
}




[System.Serializable]
public class PlayerData
{
    public string Name;
    public string Variablename;
    public float Offensepower;
    public float Defense;
    public float HP;
    public float Speed;
    public string Objectname;
    public string Animatorname;
    public string Prefabname;
}