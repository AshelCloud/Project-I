using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Player myPlayer;
    private IDamageable enemy;

    private Collider2D _collider;
    public Collider2D myCollider
    {
        get
        {
            if (_collider == null)
            {
                _collider = GetComponent<Collider2D>();
            }
            return _collider;
        }
    }


    private void Awake()
    {
        myPlayer = GetComponentInParent<Player>();
    }

    private void Start()
    {
        if (myCollider)
        {
            myCollider.isTrigger = true;
            myCollider.enabled = false;
        }

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) { return; }

        enemy = collision.GetComponentInParent<IDamageable>();

        if (enemy != null)
        {
            if (myPlayer.GetComponent<IDamageable>() == enemy) { return; }

            enemy.GetDamaged(myPlayer.OffensePower);
        }
    }
}
