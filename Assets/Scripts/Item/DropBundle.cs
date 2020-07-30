using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DropBundle : MonoBehaviour
{
    [System.Serializable]
    private class DropBundleData
    {
        public string DropBundleName = null;
        public int[] Quantity = new int[4] { 0, 0, 0, 0 };
        public int[] ItemID = new int[4] { 0, 0, 0, 0 };
        public int[] Percentage = new int[4] { 0, 0, 0, 0 };
        public string ProbabilitySum = null;
    }

    private DropBundleData bundleData;

    private int ID = 1;

    private string dropBundleName = null;
    private int[] quantity = new int[4] { 0, 0, 0, 0 };
    private int[] itemID = new int[4] { 0, 0, 0, 0 };
    private int[] percentage = new int[4] { 0, 0, 0, 0 };
    private string probabilitySum = null;

    private void Awake()
    {
        LoadToJsonData(ID);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

        TextAsset json = localAssetBundle.LoadAsset<TextAsset>("Characters_Table");

        //Json 파싱
        var bundleDatas = JsonManager.LoadJson<Serialization<string, DropBundleData>>(json).ToDictionary();

        //ID 값으로 해당되는 Data 저장
        //ID는 각 몬스터 스크립트에서 할당
        bundleData = bundleDatas[ID.ToString()];
    }
}
