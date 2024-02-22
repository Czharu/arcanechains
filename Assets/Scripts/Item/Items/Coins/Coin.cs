using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public LayerMask groundLayer; // Assign the ground layer in the inspector

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // This method is called when the coin collider makes contact with another collider/rigidbody
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the coin collides with the ground layer
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {

            rb.velocity = new Vector2(0, rb.velocity.y);
            rb.constraints = RigidbodyConstraints2D.FreezePositionX; // Freeze movement on the X axis
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the coin leaves the ground layer
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {

            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Keep rotation frozen
        }
    }
}