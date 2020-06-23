using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MonsterBehaviour
{
    Idle,
    Run,
    Chase,
    Attack,
    Hit,
    Dead
}

public class MBehaviourStack
{
    private Stack<MonsterBehaviour> _behaviours;
    private Stack<MonsterBehaviour> Behaviours
    {
        get
        {
            if(_behaviours == null)
            {
                _behaviours = new Stack<MonsterBehaviour>();
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

    private Dictionary<MonsterBehaviour, int> _prioritys;
    private Dictionary<MonsterBehaviour, int> Prioritys 
    {
        get
        {
            if(_prioritys == null)
            {
                _prioritys = new Dictionary<MonsterBehaviour, int>();
            }
            return _prioritys;
        }

        set { _prioritys = value; }
    }

    private Dictionary<MonsterBehaviour, string> _animationNames;
    public Dictionary<MonsterBehaviour, string> AnimationNames
    {
        get
        {
            if(_animationNames == null)
            {
                _animationNames = new Dictionary<MonsterBehaviour, string>();
            }
            return _animationNames;
        }
        set { _animationNames = value; }
    }

    /// <summary>
    /// 현재 item이 상태전이에 성립되면 Push 후 true
    /// 만일 같은 상태를 Push하면 true
    /// 모두 성립되지 않으면 false
    /// </summary>
    /// <param name="item"></param>
    public bool Push(MonsterBehaviour item)
    {
        //Empty일 경우 그냥 Push
        if(0 >= Behaviours.Count)
        {
            Behaviours.Push(item);
            
            return true;
        }

        //같으면 Push하지 않고 return true
        if(item == Behaviours.Peek())
        {
            return true;
        }

        //우선순위가 높으면 Push
        if(Prioritys[item] > Prioritys[Behaviours.Peek()])
        {
            Behaviours.Push(item);

            return true;
        }

        return false;
    }

    public MonsterBehaviour Peek()
    {
        return Behaviours.Peek();
    }

    public string PeekToAnimationName()
    {
        var peek = Behaviours.Peek();

        if(AnimationNames.ContainsKey(peek))
        {
            return AnimationNames[peek];
        }
        else
        {
            return null;
        }
    }

    public MonsterBehaviour Pop()
    {
        return Behaviours.Pop();
    }

    public void Clear()
    {
        Behaviours.Clear();
    }

    /// <summary>
    /// 애니메이션 전이 우선순위 결정함수
    /// </summary>
    public void SetPrioritys(int idle = 0, int run = 1, int chase = 2, int attack = 3)
    {
        Prioritys.Add(MonsterBehaviour.Idle, idle);
        Prioritys.Add(MonsterBehaviour.Run, run);
        Prioritys.Add(MonsterBehaviour.Chase, chase);
        Prioritys.Add(MonsterBehaviour.Attack, attack);
        Prioritys.Add(MonsterBehaviour.Hit, idle + run + chase + attack);
        Prioritys.Add(MonsterBehaviour.Dead, idle + run + chase + attack + 1);
    }

    public void SetAnimationNames(string idle = "Idle", string run = "Run", string chase = "Chase", string attack = "Attack", string hit = "Hit", string dead = "Dead")
    {
        AnimationNames.Add(MonsterBehaviour.Idle, idle);
        AnimationNames.Add(MonsterBehaviour.Run, run);
        AnimationNames.Add(MonsterBehaviour.Chase, chase);
        AnimationNames.Add(MonsterBehaviour.Attack, attack);
        AnimationNames.Add(MonsterBehaviour.Hit, hit);
        AnimationNames.Add(MonsterBehaviour.Dead, dead);
    }
}
