using UnityEngine;

public class P_RollBehaviour : StateMachineBehaviour
{

    private Player player = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player enter RollBehaviour");

        player = animator.gameObject.GetComponent<Player>();

        player.isInvincible = true;

        if (player.direction.x > 0)
        {
            player.rb.AddForce(new Vector2(player.RollForce, 0.0f), ForceMode2D.Impulse);
        }

        else
        {
            player.rb.AddForce(new Vector2(-player.RollForce, 0.0f), ForceMode2D.Impulse);
        }
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.normalizedTime >= 0.99f)
        {
            animator.SetBool("IsRoll", false);
        }

    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.isInvincible = false;
        Log.Print("Player exit RollBehaviour");
    }
}
