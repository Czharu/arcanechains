using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyLife : CharacterStats
{
    [SerializeField] private GameObject coinPrefab; // Assign this in the Unity Inspector
    public int numberOfCoinsToDrop = 5; // Default to 5, set this in the Inspector for each enemy
    [SerializeField] private bool destroyParent = false; // Toggle to choose whether to destroy the parent or not

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

    public void Damage(float i, GameObject sender)
    {
        if (isDead)
            return;
        if (sender.layer == gameObject.layer)
            return;

        TakeDamage(i);
        if (currentHealth <= 0)
        {
            this.KillEnemy();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private void KillEnemy()
    {
        for (int i = 0; i < numberOfCoinsToDrop; i++)
        {
            if (coinPrefab != null)
            {
                GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
                // Get the Rigidbody2D component of the coin
                Rigidbody2D coinRb = coin.GetComponent<Rigidbody2D>();

                // Check if the Rigidbody2D component was found
                if (coinRb != null)
                {
                    coinRb.gravityScale = 1;
                    // Generate a random direction
                    Vector2 randomDirection = Random.insideUnitCircle.normalized;
                    // Ensure there's enough horizontal force
                    randomDirection.x = randomDirection.x < 0 ? Mathf.Min(randomDirection.x, -0.5f) : Mathf.Max(randomDirection.x, 0.5f);

                    // Define the force strength
                    float forceStrength = Random.Range(5f, 5f); // Adjust the range as needed

                    // Apply the force to the coin
                    Debug.Log($"Ejecting coin with direction {randomDirection} and force {forceStrength}");
                    coinRb.AddForce(randomDirection * forceStrength, ForceMode2D.Impulse);
                }

            }
        }
        if (destroyParent && transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
        Destroy(gameObject);
        }
    }


}
