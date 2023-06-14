using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    private int currentHealthPoints;
    public int maxHealthPoints = 100;
    public HealthBar healthBar;
    [SerializeField] private AudioSource deathSoundEffect;

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealthPoints = maxHealthPoints;
        healthBar.SetMaxHealth(maxHealthPoints);
    }

    /* Update is called once per frame
    void Update()
    {
        
    }
    */

    public void Heal(int i){
        if(currentHealthPoints < maxHealthPoints){
        currentHealthPoints += i;
        healthBar.SetHealth(currentHealthPoints);
        }
        else {
            healthBar.SetHealth(maxHealthPoints);
        }
    }

    private void Damage(int i){
        currentHealthPoints -= i;
        healthBar.SetHealth(currentHealthPoints);
        if(currentHealthPoints <= 0){
            DeathSequence();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Trap")){
            Damage(20);
        }
    }

    private void DeathSequence(){
        deathSoundEffect.Play();
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
    }
    private void RestartLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
