using UnityEngine;

namespace Monster
{
    public class AttackBehaviour : StateMachineBehaviour
    {
        public int AttackTrigger = 1;

        [MinMaxRange(0f, 1f)]
        public RangeFloat AttackActivation = new RangeFloat(0f, 1f);

        private Monster myMonster;

        private bool isOn, isOff;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            myMonster = animator.GetComponent<Monster>();
            myMonster.IsAttacking = true;
            myMonster.Attack = false;

            isOn = isOff = false;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            myMonster.IsAttacking = true;

            float dir = (myMonster.Target.position - myMonster.transform.position).normalized.x;
            dir = Mathf.Clamp01(dir) == 0f ? -1f : 1f;

            //TODO: 개선
            if (dir < 0f && myMonster.transform.lossyScale.x > 0f)
            {
                myMonster.SetTurn();
            }
            else if (dir > 0f && myMonster.transform.lossyScale.x < 0f)
            {
                myMonster.SetTurn();
            }

            if (isOn == false && (stateInfo.normalizedTime % 1) >= AttackActivation.minValue)
            {
                myMonster.AttackTrigger(AttackTrigger);
                isOn = true;
            }
            if (isOff == false && (stateInfo.normalizedTime % 1) >= AttackActivation.maxValue)
            {
                myMonster.AttackTrigger(0);
                isOff = true;
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            myMonster.AttackTrigger(0);
            isOn = isOff = false;
            myMonster.IsAttacking = false;
        }
    }
}