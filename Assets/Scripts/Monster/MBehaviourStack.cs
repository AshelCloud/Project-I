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

    private Dictionary<MonsterBehaviour, int> _priority;
    private Dictionary<MonsterBehaviour, int> Priority 
    {
        get
        {
            if(_priority == null)
            {
                _priority = new Dictionary<MonsterBehaviour, int>();
            }
            return _priority;
        }

        set { _priority = value; }
    }

    /// <summary>
    /// 현재 item이 상태전이에 성립되면 Push, 성립되지 않으면 return
    /// </summary>
    /// <param name="item"></param>
    public void Push(MonsterBehaviour item)
    {
        //Empty일 경우 그냥 Push
        if(0 >= Behaviours.Count)
        {
            Behaviours.Push(item);
            
            return;
        }

        // >=를 쓰지않는 이유: 같은 행동이라면 == 이기때문에 거른다.
        if(Priority[item] > Priority[Behaviours.Peek()])
        {
            Behaviours.Push(item);

            return;
        }
    }

    public MonsterBehaviour Peek()
    {
        return Behaviours.Peek();
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
    public void SetPriority(int idle = 0, int run = 1, int chase = 2, int attack = 3)
    {
        Priority.Add(MonsterBehaviour.Idle, idle);
        Priority.Add(MonsterBehaviour.Run, run);
        Priority.Add(MonsterBehaviour.Chase, chase);
        Priority.Add(MonsterBehaviour.Attack, attack);
        Priority.Add(MonsterBehaviour.Hit, idle + run + chase + attack);
        Priority.Add(MonsterBehaviour.Dead, idle + run + chase + attack + 1);
    }
}
