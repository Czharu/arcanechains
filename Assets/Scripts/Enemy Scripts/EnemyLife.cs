using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyLife : CharacterStats
{

    
    //unity attack hit reference
    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;
    [SerializeField] private bool isDead = false;
    void Start()
    {
        isDead = false;
    }

    void Update()
    {
        
    }

    public void Damage(float i, GameObject sender){
        if (isDead)
            return;
        if (sender.layer == gameObject.layer)
            return;

        TakeDamage(i);
        if(currentHealth <= 0){
            this.killEnemy();
            isDead = true;
        }
        else
        {
            OnDeathWithReference?.Invoke(sender);
        }
    }

    /*
    private void DeathSequence(){
        DestroyObject(gameObject);
    }
    */

    private void OnCollisionEnter2D(Collision2D collision) {
    }

    private void killEnemy(){
        Destroy(gameObject);
    }

    
}
