using UnityEngine;

public class P_HitBehaviour : StateMachineBehaviour
{
    private Player player = null;
    private SpriteRenderer spriteRenderer = null;

    private float bounceLength = 5;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player enter HitState");

        player = animator.gameObject.GetComponent<Player>();
        spriteRenderer = player.GetComponent<SpriteRenderer>();

        //플레이어가 피격시 공중으로 튕겨져 나감
        if (player.direction.x > 0)
        {
            var bounceForce = new Vector2(-bounceLength, 10);

            //속도 0으로 조정 후 피격 재생, 버그 방지
            player.rb.velocity = Vector2.zero;

            player.rb.AddForce(bounceForce, ForceMode2D.Impulse);
        }

        else
        {
            var bounceForce = new Vector2(bounceLength, 10);

            //속도 0으로 조정 후 피격 재생, 버그 방지
            player.rb.velocity = Vector2.zero;

            player.rb.AddForce(bounceForce, ForceMode2D.Impulse);
        }
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //피격시 빨갛게 색 변화
        spriteRenderer.color = new Color(255, 0, 0);

        animator.SetFloat("HP", player.hp);

        if (stateInfo.normalizedTime >= 0.99f)
        {
            animator.SetBool("IsHit", false);
        }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player exit HitState");

        //색 원상복구
        spriteRenderer.color = new Color(255, 255, 255);
    }
}
