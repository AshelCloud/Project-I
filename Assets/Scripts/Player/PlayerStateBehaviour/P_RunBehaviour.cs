using UnityEngine;


//!Edit
public class P_RunBehaviour : StateMachineBehaviour
{
    private Player player;
    private CapsuleCollider2D cc2D;

    private bool isOnSlope;
    private Vector2 slopeNormalPerp;
    private float slopeDownAngle;
    private float slopeDownAngleOld;

    

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player enter RunBehaviour");

        player = animator.GetComponent<Player>();
        cc2D = animator.GetComponent<CapsuleCollider2D>();
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SlopeCheck();

        float h = Input.GetAxis("Horizontal");

        player.ChangeDirection();
        if (!player.isInteration)
        {
            if (player.Grounded && !isOnSlope)
            {
                player.rb.velocity = new Vector2(h * player.Speed, player.rb.velocity.y);
            }

            else if (player.Grounded && isOnSlope)
            {
                player.rb.velocity = new Vector2(-h * player.Speed * slopeNormalPerp.x, -h * player.Speed * slopeNormalPerp.y);
            }

            else
            {
                player.rb.velocity = new Vector2(h * player.Speed, player.rb.velocity.y);
            }

            if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                player.Run = false;
            }
        }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player exit RunBehaviour");
        player.Run = false;
    }

    public void SlopeCheck()
    {
        Vector2 checkPos = player.transform.position + new Vector3(0.0f, cc2D.size.y / 4);

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        float slopeCheckDistance = 0.2f;

        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, player.transform.right, slopeCheckDistance, LayerMask.GetMask("Floor"));
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -player.transform.right, slopeCheckDistance, LayerMask.GetMask("Floor"));

        if(slopeHitFront)
        {
            isOnSlope = true;
        }

        else if(slopeHitBack)
        {
            isOnSlope = true;
        }

        else
        {
            isOnSlope = false;
        }
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

            slopeDownAngleOld = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);

            Debug.DrawRay(hit.point, hit.normal, Color.blue);
        }
    }
}
