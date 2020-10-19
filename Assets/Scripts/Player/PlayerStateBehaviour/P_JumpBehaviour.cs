using UnityEngine;
using UnityEngine.Tilemaps;

//!Edit
public class P_JumpBehaviour : StateMachineBehaviour
{
    private Player player;

    static public bool doubleJump { get; set; } = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player enter JumpBehaviour");

        player = animator.GetComponent<Player>();


        if (player.IsJumpDown && player.Platform != null)
        {
            player.Platform.rotationalOffset = 180;
            player.Platform.GetComponent<TilemapCollider2D>().enabled = false;
        }

        else if(!doubleJump)
        {
            player.RB.AddForce(Vector2.up * player.JumpForce, ForceMode2D.Impulse);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        DoubleJump(player, animator);

        if (Input.GetKeyDown(KeyCode.S) && player.CheckWall())
        {
            player.Cling = true;
            player.Jump = false;
        }


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
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player exit JumpBehaviour");

        //animator.SetBool("IsJump", false);
        player.Jump = false;

        player.IsJumpDown = false;
    }

    static public void DoubleJump(Player player, Animator animator)
    {
        if (Input.GetKeyDown(KeyCode.D) && !doubleJump)
        {
            Log.Print("Player do double jump");

            doubleJump = true;

            animator.Play("Jump");

            //더블 점프 전 y축 속도 0 설정, 벡터 합력으로 인한 슈퍼점프 방지
            player.RB.velocity = new Vector2(player.RB.velocity.x, 0f);

            player.RB.AddForce(Vector2.up * player.JumpForce, ForceMode2D.Impulse);
        }
    }
}
