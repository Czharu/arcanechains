using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive
{
    private CharacterStats characterStats;

    private int strength, dexterity, arcana, vitality, toughness, swiftness, evasion, precision;
    void Awake(){
        Debug.Log("Passive arrived");
        characterStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }

    public Passive(int strength, int dexterity, int arcana, int vitality, int toughness, int swiftness, int evasion, int precision, CharacterStats characterStats){
        Debug.Log("Passive arrived");
        this.strength = strength;
        this.dexterity = dexterity;
        this.arcana = arcana;
        this.vitality = vitality;
        this.toughness = toughness;
        this.swiftness = swiftness;
        this.evasion = evasion;
        this.precision = precision;
        this.characterStats = characterStats;
    }

    public void ActivatePassive(){
        characterStats.strength.AddModifier(strength);
        characterStats.dexterity.AddModifier(dexterity);
        characterStats.arcana.AddModifier(arcana);
        characterStats.vitality.AddModifier(vitality);
        characterStats.swiftness.AddModifier(swiftness);
        characterStats.toughness.AddModifier(toughness);
        characterStats.precision.AddModifier(precision);
    }

    public void DeactivatePassive(){
        characterStats.strength.RemoveModifier(strength);
        characterStats.dexterity.RemoveModifier(dexterity);
        characterStats.arcana.RemoveModifier(arcana);
        characterStats.vitality.RemoveModifier(vitality);
        characterStats.swiftness.RemoveModifier(swiftness);
        characterStats.toughness.RemoveModifier(toughness);
        characterStats.precision.RemoveModifier(precision);
    }

}
