using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBeam : MonoBehaviour
{

    public LayerMask playerLayer;  // Set this in the Unity Inspector to match the player's layer
    [SerializeField] private int StarBeamDamage = 10;



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