using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLifeComponent : CharacterStats

{
    [SerializeField] private Animator animator; // Attach the Animator component in the Inspector
    [SerializeField] private GameObject brokenRockPrefab; // Assign this in the Unity Inspector
    private bool isBroken = false;

    void Start()
    {
        isBroken = false;
    }

    public void Damage(float amount, GameObject sender)
    {
        if (isBroken || sender.layer == gameObject.layer)
            return;

        TakeDamage(amount);
        if (currentHealth <= 0 && !isBroken)
        {
            StartCoroutine(BreakRock());
            isBroken = true;
        }
    }

    private IEnumerator BreakRock()
    {
        // Trigger the break animation
        animator.SetTrigger("Break");

        // Wait for the animation to finish
        yield return new WaitForSeconds(1f); // Adjust this to match the length of your break animation

        // Replace the rock with a broken version (non-animated sprite)
        if (brokenRockPrefab != null)
        {
            Instantiate(brokenRockPrefab, transform.position, transform.rotation);
        }

        // Destroy the original rock
        Destroy(gameObject);
    }
}