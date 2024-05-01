using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TransitionToRoom(Vector2 roomPosition, string doorTag)
    {
        Vector3 newPosition = CalculateDoorPosition(roomPosition, doorTag);
        Debug.Log("Transition to room at: " + roomPosition + ", spawn at: " + newPosition);

        // Find the PlayerMovement component and update the position
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SetPosition(newPosition);
        }
        else
        {
            Debug.LogError("PlayerMovement component not found on any GameObject!");
        }
    }

    private Vector3 CalculateDoorPosition(Vector2 roomPosition, string doorTag)
    {
        // Adjust these values to match your room size and the correct offset
        float offsetX = 2f; // Change to match your grid size
        float offsetY = 2f; // Change to match your grid size

        switch (doorTag)
        {
            case "DoorUp":
                return new Vector3(roomPosition.x, roomPosition.y + offsetY, 0);
            case "DoorDown":
                return new Vector3(roomPosition.x, roomPosition.y - offsetY, 0);
            case "DoorLeft":
                return new Vector3(roomPosition.x - offsetX, roomPosition.y, 0);
            case "DoorRight":
                return new Vector3(roomPosition.x + offsetX, roomPosition.y, 0);
            default:
                return new Vector3(roomPosition.x, roomPosition.y, 0); // Default return to room center
        }
    }
}
