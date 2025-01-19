using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

[CreateAssetMenu]
public class DashAbility : Ability
{
    public float dashVelocity;
    public GameObject arrowPivot;
    public override void Activate(GameObject parent)
    {
        Rigidbody2D rigidbody = parent.GetComponent<Rigidbody2D>();
        Vector2 force = (faceMouse() - rigidbody.position) * dashVelocity;
        Debug.Log("DASHING");
        rigidbody.AddForce(force);
    }

    private Vector2 faceMouse()
        {

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(
            mousePosition.x,
            mousePosition.y
        );
        return direction;
    }
}


