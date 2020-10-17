using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl
{
    private Player player;

    public InputControl(Player player)
    {
        this.player = player;
    }

    public void UpdateInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            player.Run = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            player.Attack = true;
        }

        if (Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.DownArrow))
        {
            player.isJumpDown = true;
            player.Jump = true;
        }

        if (Input.GetKeyDown(KeyCode.D) && !Input.GetKey(KeyCode.DownArrow))
        {
            player.Jump = true;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            player.Roll = true;
        }
    }


}
