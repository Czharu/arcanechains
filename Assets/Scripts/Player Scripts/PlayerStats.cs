using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    // Start is called before the first frame update
    void Start()
    {
          EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem){
        if(newItem != null){
        armor.AddModifier(newItem.armorModifier);
        evasion.AddModifier(newItem.evasionModifier);
        damage.AddModifier(newItem.damageModifier);
        }

        if(oldItem != null){
        armor.RemoveModifier(newItem.armorModifier);
        evasion.RemoveModifier(newItem.evasionModifier);
        damage.RemoveModifier(newItem.damageModifier);
        }
    }

    }

