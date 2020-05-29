using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBehaviour
{
    protected Monster MObject { get; set; }

    public MBehaviour(Monster monster)
    {
        MObject = monster;
    }

    public virtual void Start()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void OnGizmo()
    {
    }
}