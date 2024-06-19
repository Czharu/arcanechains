using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public Dictionary<string, List<GameObject>> activeObjectsDictionary;

    public static ObjectPooler Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        activeObjectsDictionary = new Dictionary<string, List<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            List<GameObject> activeObjects = new List<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
            activeObjectsDictionary.Add(pool.tag, activeObjects);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        if (objectToSpawn.activeSelf)
        {
            RecycleOldestActiveObject(tag);
        }
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        activeObjectsDictionary[tag].Add(objectToSpawn);

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
    private void RecycleOldestActiveObject(string tag)
    {
        if (activeObjectsDictionary[tag].Count > 0)
        {
            GameObject oldestObject = activeObjectsDictionary[tag][0];
            oldestObject.SetActive(false);
            activeObjectsDictionary[tag].RemoveAt(0);
        }
    }
    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if (activeObjectsDictionary.ContainsKey(tag))
        {
            activeObjectsDictionary[tag].Remove(objectToReturn);
        }
        objectToReturn.SetActive(false);
    }
}
