using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Add to player's coin count or inventory
            PickUp();

            // Optionally, play a sound or effect
            
            // Destroy or deactivate the coin object
            Destroy(gameObject);
        }
    }

    private void PickUp()
    {
        // Implement pickup logic, such as incrementing coin count
        Debug.Log("Coin picked up!");
    }
}