using System;
using System.Collections.Generic;
using UnityEngine;


/*
 * 
 * 
 * 
 * 
 * 연구하면서 개발중인 사항입니다!
 * 
 * 
 * 
 * 
 */

public class InputControl
{
    private static InputControl instance = null;

    private Dictionary<KeyCode, Action> keyDictionary;
    public Dictionary<KeyCode, Action> KeyDictionary { get { return keyDictionary; } }

    private Player player;

    public static InputControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new InputControl();
            }

            return instance;
        }
    }

    private InputControl()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        keyDictionary = new Dictionary<KeyCode, Action>
        {
            { KeyCode.LeftArrow, MoveLeft },
            { KeyCode.RightArrow, MoveRight },
            { KeyCode.A, Attack },
            { KeyCode.D, Jump },
            { KeyCode.F, Roll }
        };
    }

    public void MoveLeft()
    {
        player.SetState(new RunState());
    }

    public void MoveRight()
    {
        player.SetState(new RunState());
    }

    public void Attack()
    {
        player.SetState(new AttackState());
    }

    public void Jump()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            player.isJumpDown = true;
            player.SetState(new JumpState());
        }

        else
        {
            player.SetState(new JumpState());
        }
    }

    public void Roll()
    {
        player.SetState(new RollState());
    }
}
