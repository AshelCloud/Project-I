using UnityEngine;
using UnityEngine.Tilemaps;

//!Edit
public class P_JumpBehaviour : StateMachineBehaviour
{
    private Player player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player enter JumpBehaviour");

        player = animator.GetComponent<Player>();

        //플랫폼 하강 점프
        //우선 확인
        if (player.IsJumpDown && player.Platform != null)
        {
            player.Platform.rotationalOffset = 180;
            player.Platform.GetComponent<TilemapCollider2D>().enabled = false;
        }

        //일반 점프
        else if(!player.DoubleJump)
        {
            player.RB.AddForce(Vector2.up * player.JumpForce, ForceMode2D.Impulse);
            player.Grounded = false;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        DoubleJumping(player, animator);

        //좌측이동
        if (Input.GetKey(KeyCode.RightArrow))
        {
            player.ChangeDirection();
            //플레이어 좌측 이동
            player.RB.AddForce(new Vector2(player.Speed, 0.0f), ForceMode2D.Force);
        }

        //우측이동
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            player.ChangeDirection();
            //플레이어 우측 이동
            player.RB.AddForce(new Vector2(-player.Speed, 0.0f), ForceMode2D.Force);
        }

        //제자리 점프
        else
        {
            player.ChangeDirection();
        }

        player.CheckGround();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player exit JumpBehaviour");

        player.Jump = false;

        player.IsJumpDown = false;
    }

    static public void DoubleJumping(Player player, Animator animator)
    {
        if (Input.GetKeyDown(KeyCode.D) && !player.DoubleJump)
        {
            Log.Print("Player do double jump");

            player.DoubleJump = true;

            //더블 점프 전 y축 속도 0 설정, 벡터 합력으로 인한 슈퍼점프 방지
            player.RB.velocity = new Vector2(player.RB.velocity.x, 0f);

            animator.Play("Jump");

            player.RB.AddForce(Vector2.up * player.JumpForce, ForceMode2D.Impulse);
        }
    }
}
