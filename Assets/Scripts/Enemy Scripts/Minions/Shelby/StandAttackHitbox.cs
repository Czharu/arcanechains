using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandAttackHitbox : MonoBehaviour
{
    private AttackHandler attackHandler;

    private void Start()
    {
        attackHandler = GetComponentInParent<AttackHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CharacterStats playerStats = collision.GetComponent<CharacterStats>();
            if (playerStats != null)
            {
                float damage = attackHandler.GetComponentInParent<CharacterStats>().damage.GetValue() * attackHandler.StandAttackDamageMulti;
                playerStats.TakeDamage(damage);
                gameObject.SetActive(false); // Disable the hitbox after dealing damage
            }
        }
    }
}
