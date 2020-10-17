using UnityEngine;

public class P_AttackBehaviour : StateMachineBehaviour
{
    private Player myPlayer;

    [MinMaxRange(0f, 1f)]
    public RangeFloat AttackActivation = new RangeFloat(0f, 1f);

    private bool isOn, isOff;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player enter AttackState");
     
        myPlayer = animator.GetComponent<Player>();
        isOn = isOff = false;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isOn == false && (stateInfo.normalizedTime % 1) >= AttackActivation.minValue)
        {
            myPlayer.WeaponEnable(true);
            isOn = true;
        }
        if (isOff == false && (stateInfo.normalizedTime % 1) >= AttackActivation.maxValue)
        {
            myPlayer.WeaponEnable(false);
            isOff = true;

            myPlayer.Attack = false;
        }
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player exit AttackState");
        myPlayer.Attack = false;
        myPlayer.WeaponEnable(false);
        isOn = isOff = false;
    }
}
