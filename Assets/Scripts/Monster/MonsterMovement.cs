using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

//몬스터 로직
public partial class Monster : MonoBehaviour
{
    private void Awake()
    {
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
        hash_Idle = Animator.StringToHash(m_Idle);
        hash_Attack = Animator.StringToHash(m_Attack);
        hash_Chase = Animator.StringToHash(m_Chase);
    }
    
    private bool LoadToJsonData(int ID)
    {
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
        objectName = dataTable.ObjectName;
        animatorName = dataTable.AnimatorName;
        offensePower = dataTable.OffensePower;
        defense = dataTable.Defense;
        HP = dataTable.HP;
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

    //TODO: 보스몬스터 제외
    private void Patrol()
    {
        if(IsAttacking) { return; }
        if(Idle) { return; }
        if(Chase) { return; }

        if (Time.time - patrolTime > patrolTransitionTime)
        {
            Vector3 scale = transform.lossyScale;
            scale.x = -scale.x;

            transform.localScale = scale;

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

        if(targetRay.distance > AttackRange)
        {
            SetChase(true);

           _Rigidbody.velocity = new Vector2(speed * Time.deltaTime * _Rigidbody.velocity.normalized.x, _Rigidbody.velocity.y);
        }
        else
        {
            SetChase(false);

            SetAttack();
            SetIdle(true);
        }
    }

    //감지 거리 > 공격 거리기 때문에 감지 거리로 RayCasting
    //TODO: BoxCast로 변경
    private void RayCasting()
    {
        Vector2 direction = transform.lossyScale.x < 0f ? new Vector2(-1f, 0f) : new Vector2(1f, 0f);

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, DetectionRagne);

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
        Anim.SetBool(hash_Chase, Chase);
    }

    private void OnDrawGizmos()
    {
        if(debug)
        {
            Gizmos.color = Color.red;
            
            Vector2 direction = transform.lossyScale.x < 0f ? new Vector2(-AttackRange, 0f) : new Vector2(AttackRange, 0f);

            Gizmos.DrawRay(transform.position, direction);

            Gizmos.color = Color.blue;

            direction = transform.lossyScale.x < 0f ? new Vector2(-DetectionRagne, 0f) : new Vector2(DetectionRagne, 0f);
            Vector3 pos = transform.position;
            pos.y -= 0.3f;
            Gizmos.DrawRay(pos, direction);
        }
    }
}
