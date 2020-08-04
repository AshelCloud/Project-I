using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Experimental.AI;
using System.Runtime.InteropServices.WindowsRuntime;

public class DropBundle : MonoBehaviour
{
    [System.Serializable]
    private class DropBundleData
    {
        public string DropBundleName = null;
        public List<int> Quantity;
        public List<int> ItemID;
        public List<int> Percentage;
        public string ProbabilitySum = null;
    }

    private DropBundleData bundleData;

    public int ID = 2;

    private string dropBundleName = null;
    private List<int> quantity;
    private List<int> itemID;
    private List<int> percentage;
    private string probabilitySum = null;

    private void Awake()
    {
        LoadToJsonData(ID);
        SetData();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Inventory.Instance.GetGold(DroppingBundle());
            Destroy(gameObject);
        }
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

        TextAsset json = localAssetBundle.LoadAsset<TextAsset>("Item_Bundle_Table");

        //Json 파싱
        var bundleDatas = JsonManager.LoadJson<Serialization<string, DropBundleData>>(json).ToDictionary();

        //ID 값으로 해당되는 Data 저장
        //ID는 각 몬스터 스크립트에서 할당
        bundleData = bundleDatas[ID.ToString()];
    }

    private void SetData()
    {
        dropBundleName = bundleData.DropBundleName;
        quantity = bundleData.Quantity;
        itemID = bundleData.ItemID;
        percentage = bundleData.Percentage;
        probabilitySum = bundleData.ProbabilitySum;
    }

    private int DroppingBundle()
    {
        int probability = Random.Range(1, 100);
        int cost = 0;
        for (int i = 0; i < percentage.Count; i++)
        {
            if (probability <= percentage[i])
            {
                cost = quantity[i];
                break;
            }

            else
            {
                probability -= percentage[i];
            }
        }

        Log.Print("Get Gold: " + cost.ToString());
        return cost;
    }
}


