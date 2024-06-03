using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBeam : MonoBehaviour
{

    public LayerMask playerLayer;  // Set this in the Unity Inspector to match the player's layer
    [SerializeField] private int StarBeamDamage = 10;

    private new Collider2D collider2D;
    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        if (collider2D != null)
        {
            collider2D.enabled = false;  // Disable collider at the start
        }
    }

    public void EnableColliderAfterDelay(float delay)
    {
        StartCoroutine(EnableCollider(delay));
    }

    private IEnumerator EnableCollider(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (collider2D != null)
        {
            collider2D.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            // Check if the collided object is the player
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerInteraction playerStats = collision.gameObject.GetComponent<PlayerInteraction>();
                if (playerStats != null)
                {
                    playerStats.Damage(StarBeamDamage); // Damage the player
                }
            }
        }
    }
}