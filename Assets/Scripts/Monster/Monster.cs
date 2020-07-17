using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public partial class Monster : MonoBehaviour, IDamageable
{
    //의도적으로 공백으로 비워둔 클래스 입니다.
}

[System.Serializable]
public class MonsterDataTable
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