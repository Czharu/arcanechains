using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotAimRangedWeapon : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public Transform rangedWeaponPivot; // Reference to the pivot point
    public Transform crossbowTipTransform; // Reference to the tip of the crossbow

    
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;//find player, an be set in inspector instead for performance if needed
 
    }

    void Update()
    {
        if (playerTransform != null && rangedWeaponPivot != null && crossbowTipTransform != null)
        {
            AimAtPlayer();
        }
    }

    private void AimAtPlayer()
    {
        // Calculate the direction from the crossbow tip to the player
        Vector2 directionToPlayer = playerTransform.position - crossbowTipTransform.position;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg + 180f;

        // Apply the rotation to the pivot
        rangedWeaponPivot.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
