using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Legacy
{
public partial class Monster : MonoBehaviour
{
    protected virtual void IdleStartBehaviour()
    {
    }
    protected virtual void IdleUpdateBehaviour()
    {
    }
    protected virtual void IdleOnGizmosBehaviour()
    {
    }
    protected virtual void RunStartBehaviour()
    {
    }
    protected virtual void RunUpdateBehaviour()
    {
    }
    protected virtual void RunOnGizmosBehaviour()
    {
    }
    protected virtual void ChaseStartBehaviour()
    {
    }
    protected virtual void ChaseUpdateBehaviour()
    {
    }
    protected virtual void ChaseOnGizmosBehaviour()
    {
    }
    protected virtual void AttackStartBehaviour()
    {
    }
    protected virtual void AttackUpdateBehaviour()
    {
    }
    protected virtual void AttackOnGizmosBehaviour()
    {
    }
    protected virtual void HitStartBehaviour()
    {
    }
    protected virtual void HitUpdateBehaviour()
    {
    }
    protected virtual void HitOnGizmosBehaviour()
    {
    }
    protected virtual void DeadStartBehaviour()
    {
    }
    protected virtual void DeadUpdateBehaviour()
    {
    }
    protected virtual void DeadOnGizmosBehaviour()
    {
    }
    protected Transform FindPlayer(Vector2 start, Vector2 end)
    {
        var results = Physics2D.LinecastAll(start, end);

        foreach (var result in results)
        {
            if (result.collider.CompareTag("Player"))
            {
                return result.transform;
            }
        }

        return null;
    }
}
}