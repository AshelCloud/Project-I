using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    public int AttackTrigger = 1;

    [MinMaxRange(0f, 1f)]
    public RangeFloat AttackActivation = new RangeFloat(0f, 1f);

    private Monster monster;

    private bool isOn, isOff;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monster = animator.GetComponent<Monster>();
        monster.IsAttacking = true;
        monster.Attack = false;

        isOn = isOff = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monster.IsAttacking = true;

        if (isOn == false && (stateInfo.normalizedTime % 1) >= AttackActivation.minValue)
        {
            monster.AttackTrigger(AttackTrigger);
            isOn = true;
        }
        if (isOff == false && (stateInfo.normalizedTime % 1) >= AttackActivation.maxValue)
        {
            monster.AttackTrigger(0);
            isOff = true;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monster.AttackTrigger(0);
        isOn = isOff = false;
        monster.IsAttacking = false;
    }
}
