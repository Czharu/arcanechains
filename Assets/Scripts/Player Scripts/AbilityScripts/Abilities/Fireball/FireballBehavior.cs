using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float normalFireballSpeed = 15;
    [SerializeField] private float destroyTime = 3;
    [SerializeField] private LayerMask bulletColision;

    public float damage;
    private HashSet<GameObject> hitObjects = new HashSet<GameObject>();

    private float explosionDamage;
    public float explosionRadius = 2f;

    private PlayerStats playerStats;

    private CircleCollider2D explosionCollider;
    private bool isExplosionActive = false;  // Flag to track if the explosion is active

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        setDamage();
        SetDestroyTime();
        setVelocity();


        // Create the explosion collider dynamically and set it as a trigger
        explosionCollider = gameObject.AddComponent<CircleCollider2D>();
        explosionCollider.isTrigger = true;
        explosionCollider.enabled = false; // Initially disabled
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if explosion is active, preventing fireball damage during explosion
        if (isExplosionActive)
        {
            // Handle explosion damage if the collider is within the explosion range
            if (explosionCollider.enabled && !hitObjects.Contains(collider.gameObject))
            {
                EnemyLife enemyLife = collider.GetComponent<EnemyLife>();

                if (enemyLife != null){

                    hitObjects.Add(collider.gameObject);
                    enemyLife.Damage(explosionDamage, gameObject); // Apply explosion damage
                    Debug.Log("Explosion does " + explosionDamage + " Damage to " + collider.gameObject.name);
                
                }
            }
        }
        else
        {
            // Handle the collision for the initial fireball hit
            if ((bulletColision.value & (1 << collider.gameObject.layer)) > 0)
            {
                EnemyLife enemyLife = collider.GetComponent<EnemyLife>();
                if (enemyLife != null)
                {
                    Debug.Log("Fireball does " + damage + " Damage to " + collider.gameObject.name);
                    enemyLife.Damage(damage, gameObject);
                }
                Explode();
            }
        }
    }

    private void setVelocity()
    {
        rb.velocity = transform.right * normalFireballSpeed;
    }

    private void SetDestroyTime()
    {
        Destroy(gameObject, destroyTime);
    }

    private void setDamage()
    {
        damage = playerStats.damage.GetValue(); // should be arcana
        explosionDamage = damage * 0.5f;
    }


    private void Explode()
    {
        Debug.Log("Explosion starting!");
        rb.velocity = Vector2.zero; // Stop movement
        rb.isKinematic = true; // Disable rigid body so it stays in place

        // Enable the explosion collider
        explosionCollider.enabled = true;

        // Set explosion flag to true
        isExplosionActive = true;

        // Start the explosion animation and damage application
        StartCoroutine(SmoothExplosion());
    }

    private IEnumerator SmoothExplosion()
{
    float duration = 0.5f; // Explosion lasts 0.5 seconds
    float startTime = Time.time;
    Vector3 originalScale = transform.localScale;
    Vector3 targetScale = originalScale * explosionRadius; // Grow the size
    Vector3 originalPosition = transform.position; // Save position

    // The starting radius of the explosion collider
    float originalRadius = explosionCollider.radius;
    float targetRadius = originalRadius * explosionRadius; // Double the radius to match the sprite's size

    while (Time.time < startTime + duration)
    {
        float t = (Time.time - startTime) / duration;
        transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
        transform.position = originalPosition; // Keep position locked

        // Dynamically scale the explosion collider radius as the fireball grows
        explosionCollider.radius = Mathf.Lerp(originalRadius, targetRadius, t); // Adjust the radius size to match the sprite's growth

        yield return null;
    }

    Destroy(gameObject); // Destroy the fireball after the explosion
}
}