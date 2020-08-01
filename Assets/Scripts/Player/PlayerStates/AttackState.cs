using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 공격 상태
public class AttackState : IPlayerState
{
    private Player player;
    private float currentAnimTime;

    //공격 애니메이션 1 ~ 4
    private int currentAnim;

    //현재 재생되는 애니메이션을 비교해 공격 판정을 애니메이션당 한번만 실행하기 위함
    private int attackAnim;

    void IPlayerState.OnEnter(Player player)
    {
        Log.Print("Enter AttackState");
        this.player = player;
        player.Sword.SetActive(true);
        currentAnim = 1;
        attackAnim = 1;
    }

    //공격 상태에 따른 행동들
    void IPlayerState.Update()
    {
        currentAnimTime = player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

        //1~4까지의 공격 애니메이션 실행
        player.anim.Play("Attack" + currentAnim.ToString());

        //공격 판정 검사 및 실행
        if ((currentAnimTime >= 0.6f && currentAnimTime < 0.7f) &&          //검을 휘두르는 스프라이트에 맞춰 몬스터 타격
           attackAnim == currentAnim)                                       //현재 애니메이션 비교
        {
            //다음 애니메이션에 대한 공격 판정을 위해 증가
            attackAnim++;
            Log.Print("플레이어 공격 단계: " + currentAnim + "단계 공격");
            //몬스터 타격
            SwordHitMonster();
        }

        //현재 애니메이션이 끝나면 다음 단계 애니메이션으로 전이
        else
        {
            if (currentAnimTime >= 0.99f &&
                player.anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack1"))
            {
                //다음 애니메이션 시작을 위해 갱신
                currentAnim = 2;
            }

            else if (currentAnimTime >= 0.99f &&
                     player.anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack2"))
            {
                currentAnim = 3;
            }

            else if (currentAnimTime >= 0.99f &&
                     player.anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack3"))
            {
                currentAnim = 4;
            }

            else if (currentAnimTime >= 0.99f &&
                     player.anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack4"))
            {
                currentAnim = 1;
                attackAnim = 1;
            }

            else
            { }
        }
        //애니메이션 종료 후 아무 입력이 없으면 대기 상태로 전이
        if (!Input.GetKey(KeyCode.A) && currentAnimTime >= 0.99f)
        {
            player.SetState(new IdleState());
        }

    }

    void IPlayerState.OnExit()
    {
        Log.Print("Exit AttackState");
        //공격 애니메이션 최초 단계로 초기화

        player.Sword.SetActive(false);
        currentAnim = 1;
        attackAnim = 1;

        //몬스터 해제
        player.hitTarget = null;
    }

    private void SwordHitMonster()
    {
        if (player.hitTarget)
        {
            player.hitTarget.GetDamaged(player.offensePower);
            Log.Print("Monster HP: " + player.hitTarget.HP);
        }

        else
        {
        }
    }
}