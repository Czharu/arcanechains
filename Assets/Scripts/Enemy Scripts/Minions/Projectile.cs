using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private bool flip180; // Should the projectile be flipped 180 degrees?

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        RotateToFaceTrajectory();
    }

    private void RotateToFaceTrajectory()
    {
        if (rb.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            if (flip180)
            {
                angle += 180; // Adjust angle if the projectile should be flipped
            }
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
