using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    private void NPCInteraction(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<NPC>() != null)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                collision.GetComponent<NPC>().Conversation();
            }
        }

        if(collision.gameObject.GetComponent<ShopKeeper>() != null)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                collision.GetComponent<ShopKeeper>().ShopOpen();
            }
        }
    }
}
