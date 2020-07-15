using System.IO;
using TMPro;
using UnityEngine;

//몬스터 로직
public partial class Monster : MonoBehaviour
{
    protected virtual void GetHashIDs()
    {
        hash_Idle = Animator.StringToHash(m_Idle);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();

        GetHashIDs();

        if(onAwakeIdle == false)
        {
            idle = false;
        }

        patrolTime = Time.time;

        //TODO: ID 만들기
        bool result = LoadToJsonData(ID);
        if(result == false)
        {
            Log.PrintError("Not Initialize Monster In Awake Function");
        }

        DataTableLinking();
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
        
    }

    private void Update()
    {
        Patrol();

        RayCasting();
    }

    //TODO: 보스몬스터 제외
    private void Patrol()
    {
        if(Time.time - patrolTime > patrolTransitionTime)
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

    //TODO: BoxCast로 변경
    private void RayCasting()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right, detectionRange);

        foreach(RaycastHit2D hit in hits)
        {
            bool isPlayer = hit.collider.gameObject.GetComponent<Player>();

            if(isPlayer)
            {
                target = hit.transform;

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
    }

    private void OnDrawGizmos()
    {
        if(debug)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + detectionRange, transform.position.y, transform.position.z));
        }
    }
}
