using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   public float followSpeed = 2f;
   public Transform target;
    private void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            if (target == null)
            {
                Debug.LogError("Player not found: Check if the player is tagged correctly and active in the scene.");
            }
        }
    }
    private void Update()
    {
        if (target != null)
        {
            Vector3 newPos = new Vector3(target.position.x, target.position.y, -10);  // Ensure the camera stays behind the scene in a 2D setup
            transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);  // Use Time.deltaTime to make movement smooth and frame rate independent
        }
    }
}
