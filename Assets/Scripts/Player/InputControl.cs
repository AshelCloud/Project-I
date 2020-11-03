using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void Update()
    {

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            player.Run = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            player.Attack = true;
        }

        if(Input.GetKeyDown(KeyCode.D) && player.Grounded)
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                player.IsJumpDown = true;
                player.Jump = true;
            }

            if (!Input.GetKey(KeyCode.DownArrow))
            {
                player.Jump = true;
            }
        }


        if (Input.GetKeyDown(KeyCode.F))
        {
            player.Roll = true;
        }

        if (Input.GetKeyDown(KeyCode.S) && player.CheckWall())
        {
            player.Cling = true;
            player.Jump = false;
        }
    }


}
