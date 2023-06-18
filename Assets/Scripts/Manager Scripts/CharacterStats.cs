
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
   [SerializeField] public int maxHealth = 100;
   [SerializeField] public int currentHealth { get; private set; }
   public Stat damage;
   public Stat armor;
   public Stat evasion;

   private void Awake() {
    currentHealth = maxHealth;
   }

   private void Update() {

   }

   public void TakeDamage(int damage){
    if(calcHiterat() == true){
    damage -= armor.GetValue();
    damage = Mathf.Clamp(damage, 0, int.MaxValue);
    currentHealth -= damage;
    if(currentHealth <= 0){
        Die();
    }
   }
   }

       public void Heal(int i){
        if(currentHealth < maxHealth){
        currentHealth += i;
        }
    }

    private bool calcHiterat(){
        float rnd = Random.Range(0f, 100f);
        if(rnd >= evasion.GetValue()){
        return true;
    } else {
        return false;
    }
    }


    public virtual void Die(){
        //Meant to be overwritte
        Debug.Log(transform.name +  "died");
    }
}

