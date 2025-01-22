using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightningStrikeBehaviour : MonoBehaviour
{

    [SerializeField] private float destroyTime = 4f;
    [SerializeField] private LayerMask bulletColision;
    [SerializeField] private float lightnintStrikeSpeed = 40;
    [SerializeField] private float scatterDuration = 0.3f; // Duration for expanding collider
    [SerializeField] private float scatterWidthMultiplier = 2f; // Multiplier for collider size during scatter

    private Rigidbody2D rb;
    public float damage;
    private HashSet<GameObject> hitObjects = new HashSet<GameObject>();
    private BoxCollider2D boxCollider;
    private Vector2 originalColliderSize;
    private PlayerStats playerStats;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
        boxCollider = GetComponent<BoxCollider2D>();
        originalColliderSize = boxCollider.size;
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        setDamage();
        setVelocity();
        setDestroyTime();
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if (!hitObjects.Contains(collider.gameObject))
            {
                EnemyLife enemyLife = collider.GetComponent<EnemyLife>();

                if (enemyLife != null){

                    hitObjects.Add(collider.gameObject);
                    enemyLife.Damage(damage, gameObject); // Apply explosion damage
                    Debug.Log("Lightning Strike does " + damage + " Damage to " + collider.gameObject.name);
                
                }
            }
        if ((bulletColision.value & (1 << collider.gameObject.layer)) > 0)
            {
                StartCoroutine(GradualScatter());
            }
    }

    private void setDestroyTime()
    {
        Destroy(gameObject, destroyTime);
    }

    private void setVelocity()
    {
        rb.velocity = transform.right * lightnintStrikeSpeed;
    }
    private void setDamage()
    {
        damage = playerStats.damage.GetValue(); // should be arcana
    }


    private IEnumerator GradualScatter()
    {
        // Stop the lightning bolt's movement
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        // Gradually expand the collider width
        float elapsedTime = 0f;
        while (elapsedTime < scatterDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / scatterDuration;

            // Interpolate the collider size
            float newWidth = Mathf.Lerp(originalColliderSize.y, originalColliderSize.y * scatterWidthMultiplier, progress);
            boxCollider.size = new Vector2(originalColliderSize.x, newWidth);

            yield return null; // Wait for the next frame
        }

        // Destroy the game object after the scatter
        Destroy(gameObject);
    }
}

