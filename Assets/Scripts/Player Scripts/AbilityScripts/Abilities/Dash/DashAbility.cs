using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu]
public class DashAbility : Ability
{
    public float dashVelocity = 14f;
    public GameObject arrowPivot;
    public override void Activate(GameObject parent)
    {
        Rigidbody2D rb = parent.GetComponent<Rigidbody2D>();
        Vector2 direction = (faceMouse() - (Vector2)parent.transform.position).normalized;
        Debug.Log("DASHING in direction: " + direction + " With intial velocity: " + rb.velocity);
        rb.AddForce(direction * dashVelocity);
        Debug.Log("Velocity after dash : " + rb.velocity);
    }

    private Vector2 faceMouse()
    {  
    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    return mousePosition;
    }
}


