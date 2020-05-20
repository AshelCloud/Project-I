using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MPatrol : MBehaviour
{
    public MPatrol(GameObject go):
        base(go)
    {
    }

    public override void Start()
    {
        Debug.Log("Patrol Start");
    }

    public override void Update()
    {
        Debug.Log(MObject.transform.position);
    }
}