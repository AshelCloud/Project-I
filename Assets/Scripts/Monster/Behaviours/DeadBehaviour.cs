using UnityEngine;

public class DeadBehaviour : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        var bundle = ResourcesContainer.Load<DropBundle>("Prefabs/Item/DropBundle");

        bundle.ID = animator.gameObject.GetComponent<Monster>().DropBundleID;
        var Drop = Instantiate(bundle, animator.gameObject.transform.position, Quaternion.identity);

        if(bundle != null)
        {
            Log.Print("Bundle dropped!");
        }

        else
        {
            Log.PrintError("Error: Failed to drop the bundle!");
        }

        Destroy(animator.gameObject);
    }
}
