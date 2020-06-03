using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBehaviour
{
    public string BehaivorName { get; set; }
    public Action Start { get; set; }
    public Action Update { get; set; }
    public Action OnGizmos { get; set; }
}