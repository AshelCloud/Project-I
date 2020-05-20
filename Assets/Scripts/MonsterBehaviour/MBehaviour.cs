using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBehaviour
{
    protected GameObject MObject { get; set; }

    public MBehaviour(GameObject go)
    {
        MObject = go;
    }

    public virtual void Start()
    {
    }

    public virtual void Update()
    {
    }
}