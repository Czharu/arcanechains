using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The PlayerStats script manages the player's health.
public class PlayerStats : CharacterStats
{
    public PlayerInteraction playerInteraction; // Reference to the PlayerInteraction script
    private PlayerMovement playerMovement; // Reference to the PlayerMovement script
    // Start is called before the first frame update
    void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
        playerInteraction = GetComponent<PlayerInteraction>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // Ensure current health is synchronized
        playerInteraction.currentHealthPoints = currentHealth;

    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        playerInteraction.UpdateHealthBar(currentHealth); // Update the health bar
        if (currentHealth <= 0 && playerMovement.HasFollowers()) // folower  taking the hit logic
        {
            playerMovement.TriggerFollowerSave(transform.position);
        }
        else if (currentHealth <= 0)
        {
            playerInteraction.DeathSequence();
        }
    }

    public override void Heal(float amount)
    {
        base.Heal(amount);
        playerInteraction.UpdateHealthBar(currentHealth); // Update the health bar
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
            armor.AddModifier(newItem.armorModifier);
            evasion.AddModifier(newItem.evasionModifier);
            damage.AddModifier(newItem.damageModifier);
            attackSpeed.AddModifier(newItem.attackspeedModifier);
            if(newItem.weaponType != WeaponType.NonWeapon){
                weaponTypeEquipped = newItem.weaponType;
            }
        }

        if (oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorModifier);
            evasion.RemoveModifier(oldItem.evasionModifier);
            damage.RemoveModifier(oldItem.damageModifier);
            attackSpeed.RemoveModifier(oldItem.attackspeedModifier);
        }
    }

}

