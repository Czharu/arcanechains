using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimationController : MonoBehaviour
{
    public GameObject enemiesParent; // Reference to the Enemies GameObject to be set in the room prefab
    public GameObject leftHalfOfDoor; // Reference to the left half of the door
    public GameObject rightHalfOfDoor; // Reference to the right half of the door
    public BoxCollider2D terrainBoxCollider; // Reference to the TerrainBoxCollider
    public BoxCollider2D doorTriggerCollider; // Reference to the DoorDown BoxCollider

    private Animator leftDoorAnimator; // Animator for the left half of the door
    private Animator rightDoorAnimator; // Animator for the right half of the door
    private bool isDoorClosed = false; // Track if the door is currently closed


    void Start()
    {
        leftDoorAnimator = leftHalfOfDoor.GetComponent<Animator>();
        rightDoorAnimator = rightHalfOfDoor.GetComponent<Animator>();

        // Ensure the door starts open
        leftHalfOfDoor.SetActive(false);
        rightHalfOfDoor.SetActive(false);
        terrainBoxCollider.enabled = false;
        doorTriggerCollider.enabled = true;
    }

    void Update()
    {
        CheckEnemiesAndUpdateDoor();
    }

    void CheckEnemiesAndUpdateDoor()
    {
        bool hasEnemies = enemiesParent.transform.childCount > 0;

        if (hasEnemies)
        {
            // Close the door
            leftHalfOfDoor.SetActive(true);
            rightHalfOfDoor.SetActive(true);
            leftDoorAnimator.SetTrigger("Close");
            rightDoorAnimator.SetTrigger("Close");
            terrainBoxCollider.enabled = true;
            doorTriggerCollider.enabled = false;
            isDoorClosed = true;
        }
        else if (!hasEnemies && isDoorClosed)
        {
            // Keep the door open
            leftDoorAnimator.SetTrigger("Open");
            rightDoorAnimator.SetTrigger("Open");
            terrainBoxCollider.enabled = false;
            doorTriggerCollider.enabled = true;
            isDoorClosed = false;
            StartCoroutine(DeactivateDoorHalvesAfterAnimation());
        }
    }
    private IEnumerator DeactivateDoorHalvesAfterAnimation()
    {
        // Wait for the length of the open animation
        yield return new WaitForSeconds(leftDoorAnimator.GetCurrentAnimatorStateInfo(0).length);

        leftHalfOfDoor.SetActive(false);
        rightHalfOfDoor.SetActive(false);
    }
}