using UnityEngine;

public class P_ParryingBehaviour : StateMachineBehaviour
{
    private Player player = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player enter ParryingBehaviour");

        player = animator.GetComponent<Player>();
        player.UseShield(true);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player.Block)
        {
            //버그 방지
            player.Hit = false;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player exit ParryingBehaviour");

        player.Parrying = false;
        player.UseShield(false);
    }
}
