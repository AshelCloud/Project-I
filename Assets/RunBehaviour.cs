﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunBehaviour : StateMachineBehaviour
{
    private Player player = null;

    private bool isOnSlope;
    private Vector2 slopeNormalPerp;
    private float slopeDownAngle;
    private float slopeDownAngleOld;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player enter RunBehaviour");
        player = animator.gameObject.GetComponent<Player>();
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SlopeCheck();
        //player.rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * 0.05f, player.rb.velocity.y), ForceMode2D.Impulse);

        if (Input.GetAxis("Horizontal") > 0)
        {
            player.direction = new Vector2(Mathf.Abs(player.direction.x), player.direction.y);
        }

        else
        {
            player.direction = new Vector2(-Mathf.Abs(player.direction.x), player.direction.y);
        }

        if (player.isGrounded() && !isOnSlope)
        {
            player.rb.velocity = new Vector2(Input.GetAxis("Horizontal") * player.Speed, 0.0f);
        }

        else if (player.isGrounded() && isOnSlope)
        {
            player.rb.velocity = new Vector2(-Input.GetAxis("Horizontal") * player.Speed * slopeNormalPerp.x,
                                             -Input.GetAxis("Horizontal") * player.Speed * slopeNormalPerp.y);
        }

        else
        {
            animator.SetBool("IsJump", true);
        }

        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("IsRun", false);
        }

        else if(Input.GetKeyDown(KeyCode.D))
        {
            animator.SetBool("IsJump", true);
        }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player exit RunBehaviour");
        
    }

    public void SlopeCheck()
    {
        Vector2 checkPos = player.transform.position + new Vector3(0.0f, player.cc2D.size.y / 4);

        //SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {

    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        float slopeCheckDistance = 0.2f;
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, LayerMask.GetMask("Floor"));

        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != slopeDownAngleOld)
            {

                isOnSlope = true;
            }

            //else
            //{
            //    isOnSlope = false;
            //    Log.Print("On Slope state: " + isOnSlope);
            //}

            slopeDownAngleOld = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);

            Debug.DrawRay(hit.point, hit.normal, Color.blue);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
