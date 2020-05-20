using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MPatrol : MBehaviour
{
    public MPatrol(Monster monster, float speed):
        base(monster)
    {
        SetSpeed(speed);
    }

    private float Speed { get; set; }

    public override void Start()
    {
        Debug.Log(Speed);
    }

    public override void Update()
    {
        Debug.Log(MObject.transform.position);
    }

    public void SetSpeed(float speed)
    {
        Speed = speed;
    }
}