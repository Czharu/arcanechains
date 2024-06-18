using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//The PlayerInteraction script updates the health bar and handles player death.
public class PlayerInteraction : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerStats playerStats;

    public float currentHealthPoints;
    public float maxHealthPoints = 100f;
    public HealthBar healthBar;

    [SerializeField] private AudioSource deathSoundEffect;

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
        currentHealthPoints = playerStats.maxHealth; // Initialize with max health from PlayerStats
        healthBar.SetMaxHealth(playerStats.maxHealth);
        healthBar.SetHealth(currentHealthPoints);
    }

    /* Update is called once per frame
    void Update()
    {
        
    }
    */

    public void Heal(float amount)
    {
        playerStats.Heal(amount);
    }

    public void Damage(float amount)
    {
        playerStats.TakeDamage(amount);
    }
    public void UpdateHealthBar(float currentHealth)
    {
        healthBar.SetHealth(currentHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Trap")){
            Damage(20);
        }
    }

    public void DeathSequence(){
        deathSoundEffect.Play();
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
        PlayerInstanceScript.isRespawn = true;
        SceneManager.MoveGameObjectToScene(GameObject.FindGameObjectWithTag("Player"), SceneManager.GetActiveScene());
    }
    private void RestartLevel(){
        SceneManager.LoadScene("Start Menu");
    }
}
