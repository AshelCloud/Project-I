using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    public class AttackTrigger : MonoBehaviour
    {
        public int index = 1;

        private Monster myMonster;
        private Collider2D _collider;

        private IDamageable enemy;

        public Collider2D Collider
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
            myMonster = GetComponentInParent<Monster>();
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
            if (collision.isTrigger) { return; }

            enemy = collision.GetComponentInParent<IDamageable>();

            if (enemy != null)
            {
                if (myMonster.GetComponent<IDamageable>() == enemy) { return; }

                enemy.GetDamaged(myMonster.OffensePower);
            }
        }
    }
}