using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMouse : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    public GameObject arrowPivot; // Reference to the empty GameObject acting as the pivot point

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

        arrowPivot.transform.up = direction;

        // Anchor the bottom of the arrow sprite to the center position of the player
        Vector3 playerCenterPosition = player.transform.position;
        float playerHeight = player.GetComponent<SpriteRenderer>().bounds.extents.y;

        Vector3 arrowBottomPosition = playerCenterPosition - arrowPivot.transform.up * playerHeight;
        arrowBottomPosition -= 0.3f * Vector3.up; // Lower 0.3 units to the y-coordinate
        arrowPivot.transform.position = new Vector3(arrowBottomPosition.x, arrowBottomPosition.y, playerCenterPosition.z);
    }
}
