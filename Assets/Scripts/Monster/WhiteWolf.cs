using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class WhiteWolf : Monster
{
    private Player _player;
    private Player Player
    {
        get
        {
            if(_player == null)
            {
                _player = FindObjectOfType(typeof(Player)) as Player;
            }
            
            return _player;
        }
    }
    protected override void SetID()
    {
        ID = 2;
    }

    protected override void SetBehaviourStackSettings()
    {
        BehaviourStack.SetPrioritys();

        //넣지않은 애니메이션: Claw
        BehaviourStack.SetAnimationNames("Ready", "Run", "Run", "Bite", "Hit", "Dead");
    }


    protected override void ChaseUpdateBehaviour()
    {
        if( BehaviourStack.Push(MonsterBehaviour.Chase) == false)
        {
            return;
        }

        Anim.speed = 1.5f;
        Vector3 direction = Player.transform.position - transform.position;
        var curSclae = transform.localScale;

        float spd = Speed;
        if (direction.normalized.x < 0)
        {
            spd = -Speed;
            if (curSclae.x > 0f)
            {
                transform.localScale = new Vector3(-curSclae.x, curSclae.y, curSclae.z);
            }
        }
        else
        {
            if (curSclae.x < 0f)
            {
                transform.localScale = new Vector3(-curSclae.x, curSclae.y, curSclae.z);
            }
        }

        //추적은 1.5배 빠르게 이동
        RB.velocity = new Vector2(spd * 1.5f * Time.deltaTime, RB.velocity.y);
    }
}
