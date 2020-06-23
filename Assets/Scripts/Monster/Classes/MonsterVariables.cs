using System.Collections.Generic;
using UnityEngine;

public partial class Monster : MonoBehaviour
{
    //데이터 클래스
    //테이블에 따라 변화
    [System.Serializable]
    protected class MonsterData
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
    //원본 데이터는 그대로 보존
    protected MonsterData Data { get; set; }

    //-----------데이터 변수들--------------------
    public float OffentPower { get; set; }
    public float Defense { get; set; }
    public float HP { get; set; }
    public float Speed { get; set; }
    public float DetectionRange { get; set; }
    public float AttackRange { get; set; }
    //-----------------------------------------
    protected int ID { get; set; }
    protected Dictionary<string, MBehaviour> Behaviours { get; set; }
    public Animator Anim { get; set; }
    public SpriteRenderer Renderer { get; set; }
    public Rigidbody2D RB { get; set; }
    public MBehaviourStack BehaviourStack { get; set; }
    public bool Dead { get; set; }
}
