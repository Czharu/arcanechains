using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateWeaponOnClick : MonoBehaviour
{
    public SpriteRenderer characterRenderer, weaponRenderer;
    public GameObject player; // Reference to the player GameObject
    public GameObject arrowPivot; // Reference to the empty GameObject acting as the pivot point
    public GameObject arrow; // Reference to the arrow GameObject
    public float delay = 0.3f;
    private bool attackBlocked;

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        faceMouse();
    }


    private void faceMouse()
    {

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(
            mousePosition.x - arrowPivot.transform.position.x,
            mousePosition.y - arrowPivot.transform.position.y
        );

        Vector2 scale = transform.localScale;
        arrowPivot.transform.right = direction;
        if (direction.x < 0)
        {
            scale.y = -1;
        }
        else if (direction.x > 0)
        {
            scale.y =1;
        }
        transform.localScale = scale;

        // put weapon behind head layer
        if(transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }
    }
    public void Attack()
    {
        if (attackBlocked)
            return;
        animator.SetTrigger("AttackLmb");
        attackBlocked = true;
        StartCoroutine(DelayAttack());
        Debug.Log("attack");
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }


}
