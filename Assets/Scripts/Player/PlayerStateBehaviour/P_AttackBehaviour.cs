using UnityEngine;

public class P_AttackBehaviour : StateMachineBehaviour
{
    private Player player = null;

    private GameObject sword = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player enter AttackState");

        player = animator.gameObject.GetComponent<Player>();

        sword = player.transform.GetChild(0).gameObject;
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!Input.GetKey(KeyCode.A))
        {
            player.Attack = false;
        }

        if (stateInfo.normalizedTime >= 0.2f && stateInfo.normalizedTime < 0.21f)
        {
            SwordHitMonster();
        }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player exit AttackState");
    }

    private void SwordHitMonster()
    {
        if (player.hitTarget != null)
        {
            player.hitTarget.GetDamaged(player.OffensePower);
            Log.Print("Monster HP: " + player.hitTarget.HP);
        }

        else
        {
        }
    }
}
