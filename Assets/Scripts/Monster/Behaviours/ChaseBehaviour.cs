using UnityEngine;

public class ChaseBehaviour : StateMachineBehaviour
{
    public float speedCorrection = 1f;

    private float speed;
    private Monster myMonster;

    private Rigidbody2D _Rigidbody2D;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        myMonster = animator.GetComponent<Monster>();
        speed = myMonster.Speed * speedCorrection;
        _Rigidbody2D = myMonster._Rigidbody;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _Rigidbody2D.velocity = new Vector2(_Rigidbody2D.velocity.normalized.x * speed * Time.deltaTime, _Rigidbody2D.velocity.y);
    }
}