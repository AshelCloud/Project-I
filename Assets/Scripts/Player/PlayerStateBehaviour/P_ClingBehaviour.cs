using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_ClingBehaviour : StateMachineBehaviour
{
    private Player player;

    [Header("하강 속도")]
    [SerializeField]
    private float downSpeed = 0.0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player enter ClingState");

        P_JumpBehaviour.doubleJump = false;

        player = animator.gameObject.GetComponent<Player>();
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.RB.velocity = new Vector2(0.0f, -downSpeed);

        if (Input.GetKeyDown(KeyCode.D))
        {
            player.RB.AddForce(Vector2.up * player.JumpForce / 4, ForceMode2D.Impulse);

            player.Cling = false;
        }

        if(player.Grounded)
        {
            player.Cling = false;
        }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player exit ClingState");
    }
}
