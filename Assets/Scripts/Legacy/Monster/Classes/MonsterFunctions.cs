using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

namespace Legacy
{
public partial class Monster : MonoBehaviour
{
    /// <summary>
    /// 초기화 함수
    /// </summary>
    private void Initialize()
    {
        VariablesInitialize();

        SetBehaviourStackSettings();

        SetID();

        LoadToJsonData(ID);

        SetBehaviors();

        //초기상태 나중에 기획변경시 함수화
        BehaviourStack.Push(MonsterBehaviour.Run);

        //몬스터끼리 충돌불가
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Monster"), LayerMask.NameToLayer("Monster"));
    }
    /// <summary>
    /// 애니메이션 이름과 우선순위 결정 함수. 오버라이딩해서 사용
    /// </summary>
    protected virtual void SetBehaviourStackSettings()
    {
        BehaviourStack.SetPrioritys();
        BehaviourStack.SetAnimationNames();
    }
    /// <summary>
    /// Data를 받기 위해 ID 필요. 오버라이딩해서 할당
    /// </summary>
    protected virtual void SetID()
    {
    }
    public virtual void Hit(float damage)
    {
        BehaviourStack.Push(MonsterBehaviour.Hit);

        HP -= damage;
    }
    /// <summary>
    /// 사망했을때 호출되야하는 함수
    /// </summary>
    public virtual void DestroyObject()
    {
        Dead = true;
        Destroy(gameObject);
    }
    /// <summary>
    /// GetComponent or new를 변수에 셋팅해준다.
    /// </summary>
    private void VariablesInitialize()
    {
        if(Behaviours == null)
        {
            Behaviours = new Dictionary<string, MBehaviour>();
        }

        if(BehaviourStack == null)
        {
            BehaviourStack = new MBehaviourStack();
        }

        Anim = GetComponent<Animator>();
        if (Anim == null)
        {
            Log.PrintWarning("GameObject Not Added Animator! Adding Animator In Script");

            Anim = gameObject.AddComponent<Animator>();
        }

        Renderer = GetComponent<SpriteRenderer>();
        if (Renderer == null)
        {
            Log.PrintError("GameObject Not Sprite or Haven't SpriteRenderer!");
        }

        RB = GetComponent<Rigidbody2D>();
        if (RB == null)
        {
            Log.PrintError("GameObject Haven't RigidBdoy2D!");
        }

        //안주거썽
        Dead = false;
    }
    /// <summary>
    /// Json 파싱 함수
    /// </summary>
    /// <param name="ID"></param>
    private void LoadToJsonData(int ID)
    {
        //Json 파싱
        AssetBundle localAssetBundle = AssetBundleContainer.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "jsons"));
        if (localAssetBundle == null)
        {
            Log.PrintError("LoadToJosnData: Failed to load AssetBundle!");
        }
        TextAsset monsterTable = localAssetBundle.LoadAsset<TextAsset>("MonsterTable");

        var json = JsonManager.LoadJson<Serialization<string, MonsterData>>(monsterTable).ToDictionary();

        //데이터에 몬스터에 해당하는 키가 없으면 return
        if(json.ContainsKey(ID.ToString()) == false)
        {
            Log.PrintError("Failed to Monster Data. ID is null or 0");
            return;
        }

        Data = json[ID.ToString()];

        UpdateData();
    }

    /// <summary>
    /// 파싱된 데이터를 변수에 저장
    /// </summary>
    private void UpdateData()
    {
        transform.name = Data.ObjectName;

        //원본 데이터는 건들지않고 변수에 원본 데이터를 저장
        OffentPower = Data.OffensePower;
        Defense = Data.Defense;
        HP = Data.HP;
        Speed = Data.Speed;
        DetectionRange = Data.DetectionRange;
        AttackRange = Data.AttackRange;

        //Animator Controller 할당
        //Controller는 Resources폴더 안에 넣어두고 사용
        Anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Controllers/" + Data.AnimatorName);
    }
    /// <summary>
    /// 몬스터의 행동을 Stack에 저장하는 함수.
    /// </summary>
    private void SetBehaviors()
    {
        Behaviours.Add("Idle", new MBehaviour() { Start = IdleStartBehaviour, Update = IdleUpdateBehaviour, OnGizmos = IdleOnGizmosBehaviour });
        Behaviours.Add("Run", new MBehaviour() { Start = RunStartBehaviour, Update = RunUpdateBehaviour, OnGizmos = RunOnGizmosBehaviour });
        Behaviours.Add("Chase", new MBehaviour() { Start = ChaseStartBehaviour, Update = ChaseUpdateBehaviour, OnGizmos = ChaseOnGizmosBehaviour });
        Behaviours.Add("Attack", new MBehaviour() { Start = AttackStartBehaviour, Update = AttackUpdateBehaviour, OnGizmos = AttackOnGizmosBehaviour });
        Behaviours.Add("Hit", new MBehaviour() { Start = HitStartBehaviour, Update = HitUpdateBehaviour, OnGizmos = HitOnGizmosBehaviour });
        Behaviours.Add("Dead", new MBehaviour() { Start = DeadStartBehaviour, Update = DeadUpdateBehaviour, OnGizmos = DeadOnGizmosBehaviour });
    }
}
}