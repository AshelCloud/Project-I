using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Legacy
{
public class MBehaviour
{
    public string BehaivorName { get; set; }
    public string AnimationName { get; protected set; }
    public Action Start { get; set; }
    public Action Update { get; set; }
    public Action OnGizmos { get; set; }
}
}