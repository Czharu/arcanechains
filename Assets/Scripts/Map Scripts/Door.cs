using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to attach it to each doorway 
//responsible for handling the transition between rooms
//This script can hold information about whether the door leads to another room and what the coordinates of that room are
//
public class Door : MonoBehaviour
{
    public Vector2 leadsToRoomPosition;
    public string correspondingDoorTag; // Tag to find the corresponding door in the next room

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Pass the door's tag to manage directional transitions
            GameManager.Instance.TransitionToRoom(leadsToRoomPosition, gameObject.tag);
        }
    }
}
