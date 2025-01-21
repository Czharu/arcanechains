using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

[CreateAssetMenu]
public class FireballAbility : Ability
{
    private Vector2 direction;
    [SerializeField] private GameObject fireball;
    [SerializeField] private Transform fireballSpawnpoint;
    private GameObject fireballInst;

    public override void Activate(GameObject parent)
    {
        direction = (faceMouse() - (Vector2)parent.transform.position).normalized;

        fireballInst = Instantiate(fireball, parent.transform.position, Quaternion.identity);
        fireballInst.transform.right = direction;
    }
    private void Update(){
            
        }

    private Vector2 faceMouse()
    {  
    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    return mousePosition;
    }
}
