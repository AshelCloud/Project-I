using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor.Animations;
using UnityEngine;

/*
 *  모든 몬스터 부모 클래스
 */
public class Monster : MonoBehaviour
{
    //데이터 클래스
    //테이블에 따라 변화
    [System.Serializable]
    protected class MonsterData
    {
        public string Name;
        public string VariableName;
        public string MonsterType;
        public int OffensePower;
        public int Defense;
        public int HP;
        public int Speed;
        public int DropBundleID;
        public string ObjectName;
        public string AnimatorName;
        public string PrefabName;
    }

    [System.Serializable]
    protected class TestData
    {
        public string DropBundleName;
        public List<int> Quantity;
        public List<int> ItemID;
        public List<int> Percentage;
    }

    protected enum MonsterType
    {
        Normal,
        Boss,
    }

    private MonsterData Data { get; set; }
    protected int ID { get; set; }
    protected MonsterType Type { get; set; }
    protected Animator Anim { get; set; }

    protected virtual void Awake()
    {
        //TODO: GetComponent가 많으면 함수화
        Anim = GetComponent<Animator>();
        if (Anim == null)
        {
            Debug.LogError("GameObject Not Added Animator!");
        }

        SetID();

        LoadToJsonData(ID);
        UpdateData();
    }

    //Data를 받기 위해 ID 필요
    //각 몬스터 스크립트에서 오버라이딩해서 할당
    protected virtual void SetID()
    {
        ID = 0;
    }

    //Json 파싱해서 Data에 넣어주는 함수
    protected virtual void LoadToJsonData(int ID)
    {
        //테이블 ID는 1부터 시작
        //ID가 기본값이면 에러로그 출력
        if (ID == 0)
        {
            Debug.LogError("데이터 로드 실패! ID를 설정해주세요");
            return;
        }

        //Json 파싱
        var json = JsonManager.LoadJson<Serialization<string, MonsterData>>(Application.dataPath + "/Resources/MonsterJsons/", "MonsterTable").ToDictionary();
        
        //ID 값으로 해당되는 Data 저장
        //ID는 각 몬스터 스크립트에서 할당
        Data = json[ID.ToString()];
    }

    //Data값을 오브젝트에 할당
    protected virtual void UpdateData()
    {
        transform.name = Data.Name;

        //Animator Controller 할당
        //Controller는 Resources폴더 안에 넣어두고 사용
        Anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Controllers/" + Data.AnimatorName);
    }
}
