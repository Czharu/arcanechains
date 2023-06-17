using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIGroundedChase : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform playerTransform;
    private bool isChasing;
    public bool chasingRight = false;
    public float chaseDistance = 5;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(isChasing){
            if(transform.position.x > playerTransform.position.x){
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                chasingRight = false;
            }
            if(transform.position.x < playerTransform.position.x){
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                chasingRight = true;
            }
        }

        else {
            if(Vector2.Distance(transform.position, playerTransform.position) < chaseDistance){
                isChasing = true;
            }
        }
}
}
