using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=nADIYwgKHv4
public class LevelGeneration : MonoBehaviour
{
    [System.Serializable]
    public class RoomData
    {
        public Vector2 position;
        public int type; // For example, 0 could mean a normal room, 1 could mean a special room, etc.
        public bool up, down, left, right; // Door information
        //One Door
        public GameObject[] roomU_Prefabs; // Prefabs for rooms with an Up door only
        public GameObject[] roomR_Prefabs;
        public GameObject[] roomD_Prefabs;
        public GameObject[] roomL_Prefabs;
        // Two Doors
        public GameObject[] roomUR_Prefabs;
        public GameObject[] roomUD_Prefabs;
        public GameObject[] roomUL_Prefabs;
        public GameObject[] roomRD_Prefabs;
        public GameObject[] roomRL_Prefabs;
        public GameObject[] roomDL_Prefabs;
        //Three Doors
        public GameObject[] roomURD_Prefabs;
        public GameObject[] roomURL_Prefabs;
        public GameObject[] roomUDL_Prefabs;
        public GameObject[] roomRDL_Prefabs;
        //Four Doors
        public GameObject[] roomURDL_Prefabs;


        public GameObject prefab;
        // Add additional data as necessary
    }
    List<RoomData> generatedRoomData = new List<RoomData>();

    Vector2 worldSize = new Vector2(4, 4);
    Room[,] rooms;
    List<Vector2> takenPositions = new List<Vector2>();
    int gridSizeX, gridSizeY, numberOfRooms = 20;
    public GameObject roomWhiteObj;
    public GameObject[] roomPrefabs; // Array to hold various room prefabs

    void Start()
    {
        if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2))
        {
            numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
        }
        gridSizeX = Mathf.RoundToInt(worldSize.x);
        gridSizeY = Mathf.RoundToInt(worldSize.y);
        CreateRooms();
        SetRoomDoors();
        DrawMap();
        InstantiateGameplayRooms();
    }

    void CreateRooms()
    {
        //setup
        rooms = new Room[gridSizeX * 2, gridSizeY * 2];
        rooms[gridSizeX, gridSizeY] = new Room(Vector2.zero, 1);
        takenPositions.Insert(0, Vector2.zero);
        Vector2 checkPos = Vector2.zero;
        //magic numbers to cratee leses clump or stuff
        float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;
        //add rooms
        for (int i = 0; i < numberOfRooms - 1; i++)
        {
            float randomPerc = ((float)i) / (((float)numberOfRooms - 1));
            randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
            //grab new position
            checkPos = NewPosition();
            //test new position
            if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare)
            {
                int iterations = 0;
                do
                {
                    checkPos = SelectiveNewPosition();
                    iterations++;

                } while (NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);
                if (iterations >= 50)
                    print("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, takenPositions));
            }
            //finalize position
            rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = new Room(checkPos, 0);
            takenPositions.Insert(0, checkPos);
            //

            // Determine door layout
            bool up = // logic to determine if there's a door going up
            bool right = // logic for right door
            bool down = // logic for down door
            bool left = // logic for left door

            // Pick a prefab based on the door layout
            GameObject chosenPrefab = PickPrefab(up, right, down, left);


            // Create and store room data instead of instantiating the room
            RoomData newRoomData = new RoomData
            {
                position = checkPos,
                type = DetermineRoomType(), // You'll create this method to determine the room type
                up = up,
                down = down,
                left = left,
                right = right,
                prefab = chosenPrefab // Use the method to index into the prefab array
            };

            generatedRoomData.Add(newRoomData);
        }
    }

    GameObject PickPrefab(bool up, bool right, bool down, bool left)
    {
        // Your logic to pick the correct prefab based on door configuration
        // One Door
        if (up && !right && !down && !left) return roomU_Prefabs[Random.Range(0, roomU_Prefabs.Length)];
        if (!up && right && !down && !left) return roomR_Prefabs[Random.Range(0, roomR_Prefabs.Length)];
        if (!up && !right && down && !left) return roomD_Prefabs[Random.Range(0, roomD_Prefabs.Length)];
        if (!up && !right && !down && left) return roomL_Prefabs[Random.Range(0, roomL_Prefabs.Length)];
        // Two Door
        if (up && right && !down && !left) return roomUR_Prefabs[Random.Range(0, roomUR_Prefabs.Length)];
        if (up && !right && down && !left) return roomUD_Prefabs[Random.Range(0, roomUD_Prefabs.Length)];
        if (up && !right && !down && left) return roomUL_Prefabs[Random.Range(0, roomUL_Prefabs.Length)];
        if (!up && right && down && !left) return roomRD_Prefabs[Random.Range(0, roomRD_Prefabs.Length)];
        if (!up && right && !down && left) return roomRL_Prefabs[Random.Range(0, roomRL_Prefabs.Length)];
        if (!up && !right && down && left) return roomDL_Prefabs[Random.Range(0, roomDL_Prefabs.Length)];
        // Three Door
        if (up && right && down && !left) return roomURD_Prefabs[Random.Range(0, roomURD_Prefabs.Length)];
        if (up && right && !down && left) return roomURL_Prefabs[Random.Range(0, roomURL_Prefabs.Length)];
        if (up && !right && down && left) return roomUDL_Prefabs[Random.Range(0, roomUDL_Prefabs.Length)];
        if (!up && right && down && left) return roomRDL_Prefabs[Random.Range(0, roomRDL_Prefabs.Length)];
        // Four Door
        if (up && right && down && left) return roomURDL_Prefabs[Random.Range(0, roomURDL_Prefabs.Length)];
        // Add more conditions for each room type

        return defaultRoomPrefab; // Fallback if no condition is met
    }

    int DetermineRoomType()
    {
        // Logic to determine the room type
        // For now, we'll just return a random room type
        return Random.Range(0, roomPrefabs.Length);
    }

    public void InstantiateGameplayRooms() //You will need to call InstantiateGameplayRooms() when you want to generate the actual gameplay rooms, perhaps after the player finishes the planning phase or presses a 'Start Game' button.
    {
        foreach (RoomData roomData in generatedRoomData)
        {
            // Calculate the position where the room should be instantiated
            Vector3 roomPosition = new Vector3(roomData.position.x * 100, roomData.position.y * 100, 0); // Multiply by 10 or room size

            // Instantiate the room prefab at the calculated position
            Instantiate(roomData.prefab, roomPosition, Quaternion.identity);
        }
    }

    Vector2 NewPosition()
    {
        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;
        do
        {
            int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            bool UpDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);
            if (UpDown)
            {
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }
            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
        return checkingPos;
    }

    Vector2 SelectiveNewPosition()
    {
        int index = 0, inc = 0;
        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;
        do
        {
            inc = 0;
            do
            {
                index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
                inc++;

            } while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);
            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            bool UpDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);
            if (UpDown)
            {
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }
            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
        if (inc >= 100)
        {
            print("Error: could not find position with only one neighbor");
        }
        return checkingPos;
    }

    int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions)
    {
        int ret = 0;
        if (usedPositions.Contains(checkingPos + Vector2.right))
        {
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.left))
        {
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.up))
        {
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.down))
        {
            ret++;
        }
        return ret;
    }

    void SetRoomDoors()
    {
        for (int x = 0; x < ((gridSizeX * 2)); x++)
        {
            for (int y = 0; y < ((gridSizeY * 2)); y++)
            {
                if (rooms[x, y] == null)
                {
                    continue;
                }
                Vector2 gridPosition = new Vector2(x, y);
                if (y - 1 < 0)
                { //checks above
                    rooms[x, y].doorBot = false;
                }
                else
                {
                    rooms[x, y].doorBot = (rooms[x, y - 1] != null);
                }
                if (y + 1 >= gridSizeY * 2)
                { //checks bellow
                    rooms[x, y].doorTop = false;
                }
                else
                {
                    rooms[x, y].doorTop = (rooms[x, y + 1] != null);
                }
                if (x - 1 < 0)
                { //checks left
                    rooms[x, y].doorLeft = false;
                }
                else
                {
                    rooms[x, y].doorLeft = (rooms[x - 1, y] != null);
                }
                if (x + 1 >= gridSizeX * 2)
                { //checks right
                    rooms[x, y].doorRight = false;
                }
                else
                {
                    rooms[x, y].doorRight = (rooms[x + 1, y] != null);
                }
            }
        }
    }

    void DrawMap()
    {
        foreach (Room room in rooms)
        {
            if (room == null)
            {
                continue;
            }
            Vector2 drawPos = room.gridPos;
            drawPos.x *= 2;
            drawPos.y *= 2;
            MapSpriteSelector mapper = ObjectPersistence.Instantiate(roomWhiteObj, drawPos, Quaternion.identity).GetComponent<MapSpriteSelector>();
            mapper.type = room.type;
            mapper.up = room.doorTop;
            mapper.down = room.doorBot;
            mapper.right = room.doorRight;
            mapper.left = room.doorLeft;

        }
    }

}




