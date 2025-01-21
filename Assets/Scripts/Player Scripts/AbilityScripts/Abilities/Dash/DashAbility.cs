using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu]
public class DashAbility : Ability
{
    public float dashVelocity;
    public GameObject arrowPivot;
    public override void Activate(GameObject parent)
    {
        Rigidbody2D rigidbody = parent.GetComponent<Rigidbody2D>();
        Vector2 force = (faceMouse() - rigidbody.position).normalized * dashVelocity;
        Debug.Log("DASHING");
        rigidbody.AddForce(force);
    }

    private Vector2 faceMouse()
        {

        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        return mousePosition;
    }
}


