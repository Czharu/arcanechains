
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth { get; private set; }
   public Stat damage;
   public Stat armor;

   private void Awake() {
    currentHealth = maxHealth;
   }

   private void Update() {
    if(Input.GetKeyDown(KeyCode.T))
    {
        TakeDamage(10f);
    }
   }

   public void TakeDamage(float damage){
    damage -= armor.GetValue();
    damage = Mathf.Clamp(damage, 0, int.MaxValue);
    currentHealth -= damage;
    if(currentHealth <= 0){
        Die();
    }
   }

       public void Heal(float i){
        if(currentHealth < maxHealth){
        currentHealth += i;
        }
    }

    public virtual void Die(){
        //Meant to be overwritte
        Debug.Log(transform.name +  "died");
    }
}

