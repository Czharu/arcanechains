using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    private Path path;
    int currentWaypoint = 0;
    #pragma warning disable
    bool reachedEndOfPath = false;    
    #pragma warning restore
    private Seeker seeker;
    private Rigidbody2D rb;
    public Transform enemyGFX;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        
        InvokeRepeating("UpdatePath", 0f, 1f);
        
    }

    void UpdatePath(){
        if (seeker.IsDone()){
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p){
        if (!p.error){
            path = p;
            currentWaypoint = 0;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count){
            reachedEndOfPath = true;
            return;
        } else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

            if(force.x >= 0.01f){
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        } else if (force.x <= -0.01f){
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
