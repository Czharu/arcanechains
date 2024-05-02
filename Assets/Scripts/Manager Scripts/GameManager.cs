using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;  // Include this for Linq

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
        RoomData currentRoom = FindRoomByPosition(roomPosition);
        if (currentRoom != null)
        {
            RoomData targetRoom = FindAdjacentRoom(currentRoom, doorTag);
            if (targetRoom != null)
            {
                GameObject correspondingDoor = FindCorrespondingDoor(targetRoom, doorTag);
                if (correspondingDoor != null)
                {
                    Vector3 offset = CalculateOffset(doorTag);
                    Vector3 newPosition = correspondingDoor.transform.position + offset;
                    FindObjectOfType<PlayerMovement>().transform.position = newPosition;
                }
                else
                {
                    Debug.LogError("Corresponding door not found in the target room.");
                }
            }
        }
    }

    private RoomData FindAdjacentRoom(RoomData currentRoom, string doorTag)
    {
        Vector2 adjacentPosition = currentRoom.position;
        switch (doorTag)
        {
            case "DoorRight":
                adjacentPosition += Vector2.right;
                break;
            case "DoorLeft":
                adjacentPosition += Vector2.left;
                break;
            case "DoorUp":
                adjacentPosition += Vector2.up;
                break;
            case "DoorDown":
                adjacentPosition += Vector2.down;
                break;
        }

        return LevelGeneration.Instance.generatedRoomData.FirstOrDefault(room => room.position == adjacentPosition);
    }

    private RoomData FindRoomByPosition(Vector2 position)
    {
        if (LevelGeneration.Instance != null)
            return LevelGeneration.Instance.generatedRoomData.FirstOrDefault(room => room.position == position);
        return null;
    }
    private GameObject FindCorrespondingDoor(RoomData room, string doorTag)
    {
        switch (doorTag)
        {
            case "DoorUp": return room.doorDown;
            case "DoorDown": return room.doorUp;
            case "DoorLeft": return room.doorRight;
            case "DoorRight": return room.doorLeft;
            default: return null;
        }
    }

    private Vector3 CalculateOffset(string doorTag)
    {
        float offsetDistance = 4.0f; // Adjust as necessary to fit your game's scale
        switch (doorTag)
        {
            case "DoorUp": return new Vector3(0, -offsetDistance, 0); // Adjust direction if necessary
            case "DoorDown": return new Vector3(0, offsetDistance, 0);
            case "DoorLeft": return new Vector3(-offsetDistance, 0, 0);
            case "DoorRight": return new Vector3(offsetDistance, 0, 0);
            default: return Vector3.zero;
        }
    }
}
