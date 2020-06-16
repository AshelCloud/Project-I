using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MBehaviourStack
{
    private Stack<Monster.MonsterBehaviour> _behaviours;
    private Stack<Monster.MonsterBehaviour> Behaviours
    {
        get
        {
            if(_behaviours == null)
            {
                _behaviours = new Stack<Monster.MonsterBehaviour>();
            }

            return _behaviours;
        }
    }

    public int Count
    {
        get
        {
            return Behaviours.Count;
        }
    }

    public void Push(Monster.MonsterBehaviour item)
    {
        Behaviours.Push(item);
    }

    public Monster.MonsterBehaviour Peek()
    {
        return Behaviours.Peek();
    }

    public Monster.MonsterBehaviour Pop()
    {
        return Behaviours.Pop();
    }

    public void Clear()
    {
        Behaviours.Clear();
    }
}
