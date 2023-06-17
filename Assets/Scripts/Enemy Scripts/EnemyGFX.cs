using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyGFX : MonoBehaviour
{
    private SpriteRenderer sprite;
    private EnemyAIGroundedChase enemyTransform;

    void Start(){
        sprite = GetComponent<SpriteRenderer>();
        enemyTransform = GetComponentInParent<EnemyAIGroundedChase>();
    }
    // Update is called once per frame
    void Update()
    {
        if(enemyTransform.chasingRight == true){
            sprite.flipX = true;
        }
        else{
            sprite.flipX = false;
        }
    }
}
