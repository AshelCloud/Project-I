using System.Collections.Generic;
using System.IO;
using UnityEngine;

public partial class Player
{
    //기본값 = 1
    private const int ID = 1;

    [System.Serializable]
    private struct PlayerData
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

    private PlayerData playerData;

    [Header("플레이어 능력치")]
    [SerializeField]
    private float offensePower = 0;
    public float OffensePower { get { return offensePower; } }

    public float defense { get; private set; }

    public float hp { get; private set; } = 0f;

    public float max_HP { get; private set; } = 0f;

    //이동 속도
    [SerializeField]
    private float speed = 0f;
    public float Speed { get { return speed; } }

    //점프력
    [SerializeField]
    private float jumpForce = 0f;
    public float JumpForce { get { return jumpForce; } }

    //구르기 거리
    [SerializeField]
    private float rollForce = 0f;
    public float RollForce { get { return rollForce; } }

    public Item[] itemSocket { get; private set; } = new Item[4]
    {
        new Item("Helm"),
        new Item("Armor"),
        new Item("Accessories"),
        new Item("Weapon")
    };

    public Stack<Item> consumSocket { get; private set; } = new Stack<Item>();

    public const int consumableLimit = 5;

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
    defense = playerData.Defense;
    hp = playerData.HP;
    max_HP = playerData.HP;
    speed = playerData.Speed;
}

public bool ChangeEquipment(int socket, Item equipment)
{
    if (itemSocket[socket].itemType == equipment.itemType)
    {
        itemSocket[socket] = equipment;

        offensePower = playerData.Offensepower + itemSocket[socket].offensePower;
        defense = playerData.Defense = itemSocket[socket].defense;
        max_HP = playerData.HP + itemSocket[socket].hp;
        speed = playerData.Speed + itemSocket[socket].speed;

        return true;
    }

    else
    {
        return false;
    }
}
}

