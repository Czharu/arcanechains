using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public static LevelGeneration Instance;
    public List<RoomData> generatedRoomData = new List<RoomData>();  // Make this public or provide a public getter to access it from GameManager
    [SerializeField] private GameObject objectToActivate; // ✅ Assign this in the Inspector to ativate A_ object


    void Awake()
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

    Vector2 worldSize = new Vector2(4, 4);
    Room[,] rooms;
    List<Vector2> takenPositions = new List<Vector2>();
    int gridSizeX, gridSizeY, numberOfRooms = 20;
    public GameObject roomWhiteObj;
    public GameObject[] roomPrefabs; // Array to hold various room prefabs
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

    public GameObject[] defaultRoomPrefab;
    public GameObject[] specialRoomPrefabs; // starting room

    public GameObject endRoomU_Prefab; // ending room with Up door
    public GameObject endRoomR_Prefab; // ending room with Right door
    public GameObject endRoomD_Prefab; // ending room with Down door
    public GameObject endRoomL_Prefab; // ending room with Left door

    public GameObject prefab;
    // Add additional data as necessary

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
        InstantiateGameplayRooms();
        DrawMap();
        // ✅ Call A_ activatioon coroutine at the very end of Start()
        StartCoroutine(ActivateAfterFrame());
    }

    void CreateRooms()
    {
        //setup
        rooms = new Room[gridSizeX * 2, gridSizeY * 2];
        rooms[gridSizeX, gridSizeY] = new Room(Vector2.zero, 1); // Initialize the first room
        takenPositions.Insert(0, Vector2.zero);
        Vector2 checkPos = Vector2.zero;

        // Add the start room data directly
        RoomData startRoomData = new RoomData
        {
            position = Vector2.zero,
            type = 0,
            prefab = PickStartRoomPrefab(true, true, true, true) // Assume the start room has all doors for now, adjust as needed
        };
        generatedRoomData.Add(startRoomData);

        float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;

        //add rooms
        for (int i = 1; i < numberOfRooms; i++)
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
            int roomType = (i < numberOfRooms - 1) ? 0 : 2; // If it's the last room, set type to 2 for the end room

            rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = new Room(checkPos, roomType);
            takenPositions.Insert(0, checkPos);

            // Create and store room data instead of instantiating the room
            RoomData newRoomData = new RoomData
            {
                position = checkPos,
                type = roomType,
            };

            generatedRoomData.Add(newRoomData);
        }

        // Step 2: Assign door configurations
        for (int x = 0; x < gridSizeX * 2; x++)
        {
            for (int y = 0; y < gridSizeY * 2; y++)
            {
                if (rooms[x, y] == null) continue;

                // Determine doors based on the presence of adjacent rooms
                bool up = (y + 1 < gridSizeY * 2) && rooms[x, y + 1] != null;
                bool right = (x + 1 < gridSizeX * 2) && rooms[x + 1, y] != null;
                bool down = (y - 1 >= 0) && rooms[x, y - 1] != null;
                bool left = (x - 1 >= 0) && rooms[x - 1, y] != null;

                // Find corresponding RoomData and set its doors
                RoomData roomData = generatedRoomData.Find(r => r.position == new Vector2(x - gridSizeX, y - gridSizeY));
                if (roomData != null)
                {
                    roomData.up = up;
                    roomData.right = right;
                    roomData.down = down;
                    roomData.left = left;
                    if (roomData.type == 2)
                    {
                        roomData.prefab = PickEndRoomPrefab(up, right, down, left);
                    }
                    else if (roomData.position == Vector2.zero)
                    {
                        roomData.prefab = PickStartRoomPrefab(up, right, down, left);
                        Debug.Log($"Start Room Selected: {roomData.prefab.name}");
                    }
                    else
                    {
                        roomData.prefab = PickPrefab(up, right, down, left);
                    }
                }
            }
        }
    }

    GameObject PickPrefab(bool up, bool right, bool down, bool left)
    {
        // Your logic to pick the correct prefab based on door configuration
        // One Door
        if (up && !right && !down && !left) return roomU_Prefabs[Random.Range(0, roomU_Prefabs.Length)];
        else if (!up && right && !down && !left) return roomR_Prefabs[Random.Range(0, roomR_Prefabs.Length)];
        else if (!up && !right && down && !left) return roomD_Prefabs[Random.Range(0, roomD_Prefabs.Length)];
        else if (!up && !right && !down && left) return roomL_Prefabs[Random.Range(0, roomL_Prefabs.Length)];
        // Two Door
        else if (up && right && !down && !left) return roomUR_Prefabs[Random.Range(0, roomUR_Prefabs.Length)];
        else if (up && !right && down && !left) return roomUD_Prefabs[Random.Range(0, roomUD_Prefabs.Length)];
        else if (up && !right && !down && left) return roomUL_Prefabs[Random.Range(0, roomUL_Prefabs.Length)];
        else if (!up && right && down && !left) return roomRD_Prefabs[Random.Range(0, roomRD_Prefabs.Length)];
        else if (!up && right && !down && left) return roomRL_Prefabs[Random.Range(0, roomRL_Prefabs.Length)];
        else if (!up && !right && down && left) return roomDL_Prefabs[Random.Range(0, roomDL_Prefabs.Length)];
        // Three Door
        else if (up && right && down && !left) return roomURD_Prefabs[Random.Range(0, roomURD_Prefabs.Length)];
        else if (up && right && !down && left) return roomURL_Prefabs[Random.Range(0, roomURL_Prefabs.Length)];
        else if (up && !right && down && left) return roomUDL_Prefabs[Random.Range(0, roomUDL_Prefabs.Length)];
        else if (!up && right && down && left) return roomRDL_Prefabs[Random.Range(0, roomRDL_Prefabs.Length)];
        // Four Door
        else if (up && right && down && left) return roomURDL_Prefabs[Random.Range(0, roomURDL_Prefabs.Length)];
        // Add more conditions for each room type

        // Add a default return at the end of the method
        return defaultRoomPrefab[Random.Range(0, defaultRoomPrefab.Length)]; // Fallback if no condition is met
    }

    GameObject PickEndRoomPrefab(bool up, bool right, bool down, bool left)
    {
        if (up && !right && !down && !left) return endRoomU_Prefab; // Up door, so the end room should have a Down door
        else if (!up && right && !down && !left) return endRoomR_Prefab; // Right door, so the end room should have a Left door
        else if (!up && !right && down && !left) return endRoomD_Prefab; // Down door, so the end room should have an Up door
        else if (!up && !right && !down && left) return endRoomL_Prefab; // Left door, so the end room should have a Right door
        return defaultRoomPrefab[Random.Range(0, defaultRoomPrefab.Length)]; // Fallback if no condition is met
    }

    GameObject PickStartRoomPrefab(bool up, bool right, bool down, bool left)
    {
        // Your logic to pick the correct start room prefab based on door configuration
        // One Door
        if (up && !right && !down && !left) return specialRoomPrefabs[0]; //U
        else if (!up && right && !down && !left) return specialRoomPrefabs[1]; //R
        else if (!up && !right && down && !left) return specialRoomPrefabs[2]; //D
        else if (!up && !right && !down && left) return specialRoomPrefabs[3]; //L
        // Two Door
        else if (up && right && !down && !left) return specialRoomPrefabs[4]; //UR
        else if (up && !right && down && !left) return specialRoomPrefabs[5]; //UD
        else if (up && !right && !down && left) return specialRoomPrefabs[6]; //UL
        else if (!up && right && down && !left) return specialRoomPrefabs[7]; //RD
        else if (!up && right && !down && left) return specialRoomPrefabs[8]; //RL
        else if (!up && !right && down && left) return specialRoomPrefabs[9]; //DL
        // Three Door
        else if (up && right && down && !left) return specialRoomPrefabs[10]; //URD
        else if (up && right && !down && left) return specialRoomPrefabs[11]; //URL
        else if (up && !right && down && left) return specialRoomPrefabs[12]; //UDL
        else if (!up && right && down && left) return specialRoomPrefabs[13]; //RDL
        // Four Door
        else if (up && right && down && left) return specialRoomPrefabs[14]; //URDL
        // Add more conditions for each room type

        // Add a default return at the end of the method
        return defaultRoomPrefab[Random.Range(0, defaultRoomPrefab.Length)]; // Fallback if no condition is met
    }

    int DetermineRoomType()
    {
        // Logic to determine the room type
        // For now, we'll just return a zero
        return 0;
    }

    public void InstantiateGameplayRooms()
    {
        foreach (RoomData roomData in generatedRoomData)
        {
            Debug.Log($"Instantiating room at position {roomData.position} with prefab {roomData.prefab.name}");
            // Calculate the position where the room should be instantiated
            Vector3 roomPosition = new Vector3(roomData.position.x * 100, roomData.position.y * 100, 0); // Multiply by 10 or room size

            // Ensure the prefab is not null before instantiating
            if (roomData.prefab != null)
            {
                // Instantiate the room prefab at the calculated position
                GameObject roomInstance = Instantiate(roomData.prefab, roomPosition, Quaternion.identity);

                // Assuming doors are named and have the Door script attached
                roomData.doorUp = roomInstance.transform.Find("DoorUp")?.gameObject;
                roomData.doorDown = roomInstance.transform.Find("DoorDown")?.gameObject;
                roomData.doorLeft = roomInstance.transform.Find("DoorLeft")?.gameObject;
                roomData.doorRight = roomInstance.transform.Find("DoorRight")?.gameObject;
            }
            else
            {
                Debug.LogError($"Prefab is null for room at position {roomData.position}");
            }
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
            drawPos.x = drawPos.x * 2 + 50; //move minimap out the way
            drawPos.y = drawPos.y * 2 + 50;
            MapSpriteSelector mapper = ObjectPersistence.Instantiate(roomWhiteObj, drawPos, Quaternion.identity).GetComponent<MapSpriteSelector>();
            mapper.type = room.type;
            mapper.up = room.doorTop;
            mapper.down = room.doorBot;
            mapper.right = room.doorRight;
            mapper.left = room.doorLeft;
            Debug.Log("room type" + room.type);

        }
    }
    // ✅ New Coroutine that waits 1 frame before activating the object
    private IEnumerator ActivateAfterFrame()
    {
        yield return null; // Waits for 1 frame
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }
        else
        {
            Debug.LogWarning("objectToActivate is not assigned in LevelGeneration!");
        }
    }
}
