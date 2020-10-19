using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Player player = null;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Monster"))
        {
            player.HitTarget = collider.gameObject.GetComponent<Monster.Monster>();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Monster"))
        {
            player.HitTarget = null;
        }
    }
}
