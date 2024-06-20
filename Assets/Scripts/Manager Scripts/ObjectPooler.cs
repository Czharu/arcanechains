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
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        RecycleImpaledProjectiles(tag); // Recycles impaled projectiles first


        if (IsPoolFullyUtilized(tag)) // Check if the whole pool is used up
        {
            ExpandPool(tag, prefab);
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        //if (objectToSpawn.activeSelf)
        //{
        //    Debug.LogWarning("Object to spawn is active, expanding pool again.");
        //    ExpandPool(tag, prefab);
        //    objectToSpawn = poolDictionary[tag].Dequeue();
        //}

        // Update the objectToSpawn's components to match the prefab
        UpdateComponents(objectToSpawn, prefab);

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        // Set the pool tag for the objectToSpawn
        var projectile = objectToSpawn.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.SetPoolTag(tag);
        }

        activeObjectsDictionary[tag].Add(objectToSpawn);

        //poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    private void ExpandPool(string tag, GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return;
        }

        Queue<GameObject> objectPool = poolDictionary[tag];

        for (int i = 0; i < 1; i++) // You can adjust the number of objects to add each time the pool expands
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        Debug.Log("Expanded pool with tag: " + tag);
    }

    private bool IsPoolFullyUtilized(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return false;
        }

        // Check if all objects in the pool are active
        foreach (var obj in poolDictionary[tag])
        {
            if (!obj.activeInHierarchy)
            {
                return false;
            }
        }
        return true;
    }

    private void RecycleImpaledProjectiles(string tag)//recycleimpaledprojectiles
    {
        if (activeObjectsDictionary.ContainsKey(tag))
        {
            List<GameObject> activeObjects = activeObjectsDictionary[tag];
            for (int i = 0; i < activeObjects.Count; i++)
            {
                GameObject obj = activeObjects[i];
                var projectile = obj.GetComponent<Projectile>();
                if (projectile != null && projectile.isImpaled) // Check if the projectile is impaled
                {
                    projectile.ResetProjectile(); // Reset and deactivate the projectile
                    ReturnToPool(tag, obj); // Return it to the pool
                    break;
                }
            }
        }
    }


    private void UpdateComponents(GameObject objectToSpawn, GameObject prefab)
    {
        // Update SpriteRenderer
        var spriteRenderer = objectToSpawn.GetComponent<SpriteRenderer>();
        var prefabSpriteRenderer = prefab.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && prefabSpriteRenderer != null)
        {
            spriteRenderer.sprite = prefabSpriteRenderer.sprite;
            spriteRenderer.color = prefabSpriteRenderer.color;
            spriteRenderer.flipX = prefabSpriteRenderer.flipX;
            spriteRenderer.flipY = prefabSpriteRenderer.flipY;
        }

        // Update Rigidbody2D
        var rigidbody2D = objectToSpawn.GetComponent<Rigidbody2D>();
        var prefabRigidbody2D = prefab.GetComponent<Rigidbody2D>();
        if (rigidbody2D != null && prefabRigidbody2D != null)
        {
            rigidbody2D.gravityScale = prefabRigidbody2D.gravityScale;
            rigidbody2D.mass = prefabRigidbody2D.mass;
            rigidbody2D.drag = prefabRigidbody2D.drag;
            rigidbody2D.angularDrag = prefabRigidbody2D.angularDrag;
            rigidbody2D.interpolation = prefabRigidbody2D.interpolation;
            rigidbody2D.constraints = prefabRigidbody2D.constraints;
        }

        // Update Projectile script
        var projectile = objectToSpawn.GetComponent<Projectile>();
        var prefabProjectile = prefab.GetComponent<Projectile>();
        if (projectile != null && prefabProjectile != null)
        {
            projectile.flip180 = prefabProjectile.flip180;
            projectile.groundLayer = prefabProjectile.groundLayer;
            projectile.impaleOnCollision = prefabProjectile.impaleOnCollision;
            projectile.damage = prefabProjectile.damage;
            projectile.isImpaled = prefabProjectile.isImpaled;
        }

        // Update CapsuleCollider2D
        var collider2D = objectToSpawn.GetComponent<CapsuleCollider2D>();
        var prefabCollider2D = prefab.GetComponent<CapsuleCollider2D>();
        if (collider2D != null && prefabCollider2D != null)
        {
            collider2D.size = prefabCollider2D.size;
            collider2D.offset = prefabCollider2D.offset;
            collider2D.direction = prefabCollider2D.direction;
        }

        // Disable any components not present on the prefab
        DisableExtraComponents(objectToSpawn, prefab);
    }

    private void DisableExtraComponents(GameObject objectToSpawn, GameObject prefab)
    {
        var componentsToSpawn = objectToSpawn.GetComponents<Component>();
        var componentsInPrefab = prefab.GetComponents<Component>();

        foreach (var component in componentsToSpawn)
        {
            bool found = false;
            foreach (var prefabComponent in componentsInPrefab)
            {
                if (component.GetType() == prefabComponent.GetType())
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                if (component is Behaviour behaviour)
                {
                    behaviour.enabled = false;
                }
                else if (component is Collider2D collider)
                {
                    collider.enabled = false;
                }
                // Add more checks if needed
            }
        }
    }

    //private void RecycleOldestActiveObject(string tag)
    //{
    //    if (activeObjectsDictionary[tag].Count > 0)
    //    {
            //GameObject oldestObject = activeObjectsDictionary[tag][0];
            //oldestObject.SetActive(false);
            //activeObjectsDictionary[tag].RemoveAt(0);
        //}
    //}

    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if (string.IsNullOrEmpty(tag))
        {
            Debug.LogWarning("Trying to return an object to the pool with an empty or null tag.");
            return;
        }

        if (activeObjectsDictionary.ContainsKey(tag))
        {
            activeObjectsDictionary[tag].Remove(objectToReturn);
        }
        objectToReturn.SetActive(false);
        poolDictionary[tag].Enqueue(objectToReturn); // Ensure returned object is added back to the pool
    }
}
