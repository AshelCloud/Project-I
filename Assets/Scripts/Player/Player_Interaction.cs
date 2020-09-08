using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    public bool isInteration { get; private set; } = false;

    private Vector2 destination = Vector2.zero;

    [Header("NPC-�÷��̾� ��ȣ�ۿ� �̵� �Ÿ� ����")]
    [SerializeField]
    private float distanceFromNpc = 0f;

    private IEnumerator NPCInteraction(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (collision.gameObject.GetComponent<NPC>() != null)
            {
                isInteration = true;
                destination = collision.gameObject.transform.position - new Vector3(3f, 0f);

                yield return new WaitForSeconds(1f);

                collision.GetComponent<NPC>().Conversation();
            }

            if (collision.gameObject.GetComponent<ShopKeeper>() != null)
            {
                isInteration = true;
                destination = collision.gameObject.transform.position - new Vector3(3f, 0f);
                shopKeeper = collision.GetComponent<ShopKeeper>();

                yield return new WaitForSeconds(1f);

                shopKeeper.ShopOpen();
            }
        }
    }

    private void InterationEvent()
    {
        if(isInteration)
        {
            EnterInteration();
        }
    }

    private void EnterInteration()
    {
        Animator animator = GetComponent<Animator>();

        animator.SetBool("IsRun", true);

        Debug.Log(transform.position.x);
        Debug.Log(destination.x);

        if (transform.position.x >= destination.x)
        {
            rb.velocity = new Vector2(-Speed / 2, rb.velocity.y);
            direction = new Vector2(-Mathf.Abs(direction.x), direction.y);
        }

        else
        {
            direction = new Vector2(Mathf.Abs(direction.x), direction.y);
            animator.SetBool("IsRun", false);
            isInteration = false;
        }

    }
}
