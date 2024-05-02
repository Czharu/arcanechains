using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomData
{
    public Vector2 position;
    public int type;
    public GameObject prefab;
    public bool up, down, left, right; // Door information
    public GameObject doorUp, doorDown, doorLeft, doorRight; // Doors
}