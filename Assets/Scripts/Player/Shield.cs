using System.Runtime.CompilerServices;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private Player player;

    private Collider2D _collider;

    public Collider2D Collider
    {
        get
        {
            if(_collider == null)
            {
                _collider = GetComponent<Collider2D>();
            }

            return _collider;
        }
    }

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void Start()
    {
        if (Collider)
        {
            Collider.isTrigger = true;
            Collider.enabled = false;
        }

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemyAttackTrigger = collision.GetComponent<Monster.AttackTrigger>();

        if(enemyAttackTrigger != null && collision == enemyAttackTrigger.Collider)
        {
            player.IsInvincible = true;
            Debug.Log("Enemy Attack Block!");
            player.Block = true;
        }
    }
}
