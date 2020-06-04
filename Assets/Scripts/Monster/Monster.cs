using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
 *  모든 몬스터 부모 클래스
 */
public class Monster : MonoBehaviour
{
    //데이터 클래스
    //테이블에 따라 변화
    [System.Serializable]
    public class MonsterData
    {
        public string Name;
        public string VariableName;
        public string MonsterType;
        public float OffensePower;
        public float Defense;
        public float HP;
        public float Speed;
        public int DropBundleID;
        public string ObjectName;
        public string AnimatorName;
        public string PrefabName;
        public float DetectionRange;
        public float AttackRange;
    }

    public enum MonsterBehaviour
    {
        Idle,
        Run,
        Chase,
        Attack,
        Hit,
        Dead
    }

    protected MonsterData Data { get; set; }
    protected int ID { get; set; }
    protected Dictionary<string, MBehaviour> Behaviours { get; set; }
    public Animator Anim { get; set; }
    public SpriteRenderer Renderer { get; set; }
    public Rigidbody2D RB { get; set; }
    public float HP { get; set; }
    public float AttackRange { get; set; }
    public float DetectionRange { get; set; }

    public Stack<MonsterBehaviour> BehaviourStack { get; set; }

    public bool Dead { get; set; }

    protected virtual void Awake()
    {
        //TODO: GetComponent가 많으면 함수화
        Anim = GetComponent<Animator>();
        if (Anim == null)
        {
            Debug.LogWarning("GameObject Not Added Animator! Adding Animator In Script");

            Anim = gameObject.AddComponent<Animator>();
        }
        Renderer = GetComponent<SpriteRenderer>();
        RB = GetComponent<Rigidbody2D>();

        Behaviours = new Dictionary<string, MBehaviour>();
        BehaviourStack = new Stack<MonsterBehaviour>();

        BehaviourStack.Push(MonsterBehaviour.Run);

        SetID();

        LoadToJsonData(ID);
        UpdateData();

        SetBehaviors();

        Dead = false;
    }

    protected virtual void Start()
    {
        foreach(var action in Behaviours)
        {
            action.Value.Start?.Invoke();
        }
    }

    protected virtual void Update()
    {
        if( Dead ) { return;  }

        print(BehaviourStack.Peek());

        foreach(var action in Behaviours)
        {
            action.Value.Update?.Invoke();
        }
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
        transform.name = Data.ObjectName;

        HP = Data.HP;
        AttackRange = Data.AttackRange;
        DetectionRange = Data.DetectionRange;

        //Animator Controller 할당
        //Controller는 Resources폴더 안에 넣어두고 사용
        //Table 부재로 리터럴문자 사용
        Anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Controllers/" + Data.AnimatorName);
    }

    protected virtual void SetBehaviors()
    {
    }

    private void OnDrawGizmos()
    {
        foreach (var action in Behaviours)
        {
            action.Value.OnGizmos?.Invoke();
        }
    }

    public virtual void Hit(float damage)
    {
        BehaviourStack.Push(MonsterBehaviour.Hit);

        HP -= damage;
    }

    public virtual void DestroyObject()
    {
        Dead = true;
        Destroy(gameObject);
    }
}
