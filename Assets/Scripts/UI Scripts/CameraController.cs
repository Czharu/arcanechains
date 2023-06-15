using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   public float followSpeed = 2f;
   public Transform target;
    private void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 newPos = new Vector3(target.position.x, target.position.y, -10);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed);
    }
}
