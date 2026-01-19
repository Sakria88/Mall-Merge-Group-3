//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class FallingObjPoolerScript : MonoBehaviour
//{
//    public GameObject[] objectPrefabs; // Array of object prefabs to pool
//    public int poolSize; // Number of platforms to pool
//    public int startPrefabs; // Number of objects to spawn at start
//    public Dictionary<GameObject, bool> pool = new Dictionary<GameObject, bool>(); // Pool dictionary
//    public Transform spawnPosition; // Position to spawn platforms
//    public Transform tempPosition; // Temporary position holder

//    void Start()
//    {
//        GameObject obj;

//        for (int i = 0; i < poolSize; i++)
//        {
//            obj = Instantiate(objectPrefabs[Random.Range(0, objectPrefabs.Length)],
//                              tempPosition.position,
//                              Quaternion.identity); // Instantiate at temp position //Change proportions of different prefabs here if needed TODO

//            pool.Add(obj, true); // true indicates the object is available
//        }

//        for (int i = 0; i < startPrefabs; i++)
//        {
//            SpawnPrefab();
//        }
//        StartCoroutine(SpawnLoop());
//    }

//    void SpawnPrefab()
//    {
//        foreach (KeyValuePair<GameObject, bool> obj in pool)
//        {
//            if (obj.Value) // If the object is available
//            {
//                pool[obj.Key] = false; // Mark as in use
//                obj.Key.transform.position = spawnPosition.position; // Move to spawn position
//                NextSpawnPositionCal(obj.Key); // Calculate next spawn position
//                StartCoroutine(ReturnPrefab(obj.Key)); // Start coroutine to return object to pool
//                break; // Exit after spawning one object
//            }
//        }
//    }

//    void NextSpawnPositionCal(GameObject selPrefab)
//    {
//        spawnPosition.position = new Vector2(Random.Range(-2.25f, 2.25f),
//                                             selPrefab.transform.position.y); // Update spawn position for next object
//    }

//    IEnumerator SpawnLoop()
//    {
//        while (true)
//        {
//            float waitTime = Random.Range(0.5f, 2.0f); // Random wait time between spawns
//            SpawnPrefab(); // Spawn a new object

//            yield return new WaitForSeconds(1.5f); // Wait for 1.5 seconds
//        }
//    }

//    IEnumerator ReturnPrefab(GameObject selPrefab)
//    {
//        while (!selPrefab.GetComponent<Renderer>().isVisible)
//        {
//            yield return new WaitForEndOfFrame(); // Wait until the object is visible
//        }
//        while (selPrefab.GetComponent<Renderer>().isVisible)
//        {
//            yield return new WaitForEndOfFrame(); // Wait until the object is no longer visible
//        }

//        selPrefab.transform.position = tempPosition.position; // Move back to temp position
//        pool[selPrefab] = true; // Mark as available

//        SpawnPrefab(); // Spawn a new object
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjPoolerScript : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    public int poolSize = 10;

    public Transform spawnPosition;
    public Transform tempPosition;

    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 3f;

    private Dictionary<GameObject, bool> pool = new Dictionary<GameObject, bool>();

    void Start()
    {
        CreatePool();
        StartCoroutine(SpawnLoop());
    }

    void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(
                objectPrefabs[Random.Range(0, objectPrefabs.Length)],
                tempPosition.position,
                Quaternion.identity
            );

            obj.SetActive(false);

            // Nos suscribimos al evento
            FallingObjectScript falling = obj.GetComponent<FallingObjectScript>();
            if (falling != null)
            {
                falling.OnReturnToPool += ReturnToPool;
            }
            else
            {
                Debug.LogError(obj.name + " no tiene FallingObjectScript");
            }

            pool.Add(obj, true);
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnPrefab();
            float wait = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(wait);
        }
    }

    void SpawnPrefab()
    {
        foreach (var obj in pool)
        {
            if (obj.Value)
            {
                pool[obj.Key] = false;

                obj.Key.transform.position = GetRandomSpawnPosition();
                obj.Key.SetActive(true);

                break;
            }
        }
    }

    Vector2 GetRandomSpawnPosition()
    {
        return new Vector2(
            Random.Range(-2.25f, 2.25f),
            spawnPosition.position.y
        );
    }

    void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.position = tempPosition.position;
        pool[obj] = true;
    }
}
