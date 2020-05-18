using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class Monster : MonoBehaviour
{
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

    protected MonsterData Data { get; set; }
   protected int ID { get; set; }
   protected Animator Anim { get; set; }

    protected virtual void Awake()
    {
        Anim = GetComponent<Animator>();
        if(Anim == null)
        {
            Debug.LogError("GameObject Not Added Animator!");
        }

        SetID();

        LoadToJsonData(ID);
        UpdateData();
    }
    
    protected virtual void SetID()
    {
        ID = 0;
    }

    protected virtual void LoadToJsonData(int ID)
    {
        if(ID == 0)
        {
            Debug.LogError("데이터 로드 실패! ID를 설정해주세요");
            return;
        }
        var json = JsonManager.LoadJson<Serialization<string, MonsterData>>(Application.dataPath + "/Resources/MonsterJsons/", "MonsterTable").ToDictionary();

        Data = json[ID.ToString()];
    }

    protected virtual void UpdateData()
    {
        transform.name = Data.Name;

        Anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animators/" + Data.AnimatorName);
    }
}
