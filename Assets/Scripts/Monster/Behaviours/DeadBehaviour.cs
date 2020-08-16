using UnityEngine;

public class DeadBehaviour : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //NEED: 몬스터 번들 드랍 연동 요청

        //var bundle = new GameObject().GetComponent<DropBundle>();

        //접근 지정자로 인하여 몬스터 테이블 내의 DropBundle ID 연동 불가
        //bundle.ID = animator.gameObject.GetComponent<Monster>().DropbundleID;
        //Instantiate(bundle, animator.gameObject.transform);


        Destroy(animator.gameObject);
    }
}
