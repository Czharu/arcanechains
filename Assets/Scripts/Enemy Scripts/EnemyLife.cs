using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour
{
    public float currentHealthPoints;

    public float maxHealthPoints = 100;
    void Start()
    {
        currentHealthPoints = maxHealthPoints;
    }

    void Update()
    {
        
    }

    private void Damage(float i){
        currentHealthPoints -= i;
        if(currentHealthPoints <= 0){
            this.killEnemy();
        }
    }

    /*
    private void DeathSequence(){
        DestroyObject(gameObject);
    }
    */

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Player")){
            Damage(20);
        }
    }

    private void killEnemy(){
        Destroy(gameObject);
    }

    
}
