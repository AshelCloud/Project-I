using UnityEngine;

/// <summary>
/// Monster의 최상위 클래스
/// </summary>
public partial class Monster : MonoBehaviour
{
    protected virtual void Awake()
    {
        Initialize();
    }

    protected virtual void Start()
    {
        foreach(var action in Behaviours)
        {
            action.Value.Start?.Invoke();
        }
    }

    protected virtual void Update()
    {
        if( Dead ) { return;  }

        foreach (var action in Behaviours)
        {
            action.Value.Update?.Invoke();
        }

        //현재 애니메이션과 Stack Top에 있는 애니메이션이 다르면 Top에 있는 애니메이션 재생
        if(Anim.GetCurrentAnimatorStateInfo(0).IsName(BehaviourStack.PeekToAnimationName()) == false)
        {
            Anim.Play(BehaviourStack.PeekToAnimationName());
        }
    }

    private void OnDrawGizmos()
    {
        if(Behaviours == null)  { return; }

        foreach (var action in Behaviours)
        {
            action.Value.OnGizmos?.Invoke();
        }
    }
}
