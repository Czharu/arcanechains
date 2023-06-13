using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentyWaypointIndex = 0;
    [SerializeField] private float speed = 2f;
    private void Update()
    {
        if (Vector2.Distance(waypoints[currentyWaypointIndex].transform.position, transform.position) < 0.1f){
            currentyWaypointIndex++;
            if(currentyWaypointIndex >= waypoints.Length)
            currentyWaypointIndex = 0;
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentyWaypointIndex].transform.position, Time.deltaTime * speed);
    }
}
