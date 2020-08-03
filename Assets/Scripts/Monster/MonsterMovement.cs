using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

//몬스터 로직
public partial class Monster : MonoBehaviour
{
    private void Awake()
    {
        Log.Print("Monster: Initialization");
        anim = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();

        GetHashIDs();

        if(onAwakeIdle == false)
        {
            idle = false;
        }

        bool result = LoadToJsonData(ID);
        if(result == false)
        {
            Log.PrintError("Not Initialize Monster In Awake Function");
        }

        DataTableLinking();

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Monster"), LayerMask.NameToLayer("Monster"));
    }

    protected virtual void GetHashIDs()
    {
        Log.Print("Monster: Get Hash IDs");
        hash_Idle = Animator.StringToHash(m_Idle);
        hash_Attack = Animator.StringToHash(m_Attack);
        hash_AttackID = Animator.StringToHash(m_AttackID);
        hash_Chase = Animator.StringToHash(m_Chase);
        hash_Damaged = Animator.StringToHash(m_Damaged);
        hash_Dead = Animator.StringToHash(m_Dead);
    }
    
    private bool LoadToJsonData(int ID)
    {
        Log.Print("Monster: Load JsonData to Monster ID");
        //Json 파싱
        AssetBundle localAssetBundle = AssetBundleContainer.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "jsons"));
        if (localAssetBundle == null)
        {
            Log.PrintError("LoadToJosnData: Failed to load AssetBundle!");
            return false;
        }
        TextAsset monsterTable = localAssetBundle.LoadAsset<TextAsset>("MonsterTable");

        var json = JsonManager.LoadJson<Serialization<string, MonsterDataTable>>(monsterTable).ToDictionary();

        //데이터에 몬스터에 해당하는 키가 없으면 return
        if (json.ContainsKey(ID.ToString()) == false)
        {
            Log.PrintError("Failed to Monster Data. ID is null or 0");
            return false;
        }

        dataTable = json[ID.ToString()];
        return true;
    }

    private void DataTableLinking()
    {
        Log.Print("Monster: DataTable Linking");

        objectName = dataTable.ObjectName;
        animatorName = dataTable.AnimatorName;
        offensePower = dataTable.OffensePower;
        defense = dataTable.Defense;
        hp = dataTable.HP;
        speed = dataTable.Speed;
        detectionRange = dataTable.DetectionRange;
        attackRange = dataTable.AttackRange;
        dropBundleID = dataTable.DropBundleID;
    }

    private void Start()
    {
        AttackTriggers = GetComponentsInChildren<AttackTrigger>(true).ToList();

        attackTime = patrolTime = Time.time;
    }

    private void Update()
    {
        Patrol();

        RayCasting();

        AttackOnTarget();
    }

    private void Patrol()
    {
        if(IsAttacking) { return; }
        if(Idle || Chase) { return; }
        if(havePatrol == false) { return; }
        

        if (Time.time - patrolTime > patrolTransitionTime)
        {
            Log.Print("Monster: Change Direction Patrol");
            SetTurn();  

            patrolTime = Time.time;
        }

        int direction = transform.lossyScale.x < 0f ? -1 : 1;

        Vector2 velocity = new Vector2(speed * Time.deltaTime * direction, _Rigidbody.velocity.y);
        _Rigidbody.velocity = velocity;
    }

    private void AttackOnTarget()
    {
        if(target == null) 
        {
            SetIdle(false);
            SetChase(false);

            return; 
        }


        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > AttackRange)
        {
            SetIdle(false);
            SetChase(true);
        }
        else
        {
            SetChase(false);

            SetAttack();
            SetIdle(true);
        }
    }

    //감지 거리 > 공격 거리기 때문에 감지 거리로 RayCasting
    private void RayCasting()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, DetectionRagne, Vector2.up, 0f);
        foreach(RaycastHit2D hit in hits)
        {
            bool isPlayer = hit.collider.gameObject.GetComponent<Player>();

            if(isPlayer)
            {
                target = hit.transform;
                targetRay = hit;

                return;
            }
        }

        //찾지 못했으면 추적 종료
        target = null;
    }

    private void LateUpdate()
    {
        LinkingAnimator();
    }

    public virtual void LinkingAnimator()
    {
        Anim.SetBool(hash_Idle, Idle);
        Anim.SetBool(hash_Attack, Attack);
        Anim.SetInteger(hash_AttackID, AttackID);
        Anim.SetBool(hash_Chase, Chase);
        Anim.SetBool(hash_Damaged, Damaged);
    }

    private void OnDrawGizmos()
    {
        if(debug)
        {
            Gizmos.color = Color.blue;
            
            Gizmos.DrawWireSphere(transform.position, DetectionRagne);
            
            Gizmos.color = Color.red;
            
            Gizmos.DrawWireSphere(transform.position, AttackRange);

            //Vector2 direction = transform.lossyScale.x < 0f ? new Vector2(-AttackRange, 0f) : new Vector2(AttackRange, 0f);
            //Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction);

        }
    }
}
