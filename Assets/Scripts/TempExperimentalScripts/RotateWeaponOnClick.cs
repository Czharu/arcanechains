using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWeaponOnClick : MonoBehaviour
{
    
    public GameObject player; // Reference to the player GameObject
    public GameObject arrowPivot; // Reference to the empty GameObject acting as the pivot point
    public GameObject arrow; // Reference to the arrow GameObject
    private int currentState = 0; // Current state of the arrow rotation
    private Quaternion originalRotation; // Store the original rotation of the arrow

    // Start is called before the first frame update
    void Start()
    {
        RotateArrow(currentState); // Rotate the arrow to the initial state
    }

    // Update is called once per frame
    void Update()
    {
        faceMouse();

        if (Input.GetMouseButtonDown(0)) // Check if left mouse button is clicked
        {
            ChangeState();
        }
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

    }

    private void ChangeState()
    {
        currentState++; // Increment the state

        if (currentState > 1) // If the state exceeds the number of weapon states, reset it to 0
        {
            currentState = 0;
        }

        RotateArrow(currentState); // Rotate the arrow to the new state
    }

    private void RotateArrow(int state)
    {
        switch (state)
        {
            case 0: // State 0
                arrow.transform.rotation = originalRotation;
                break;
            case 1: // State 1
                arrow.transform.rotation = Quaternion.Euler(0, 0, -45);
                break;
            // Add more cases for additional states if needed
        }
    }

}
