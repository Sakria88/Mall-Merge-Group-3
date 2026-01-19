using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjPoolerScript : MonoBehaviour
{
    public GameObject[] objectPrefabs; // Array of object prefabs to pool
    public int poolSize; // Number of platforms to pool
    public int startPrefabs;
    public Dictionary<GameObject, bool> pool = new Dictionary<GameObject, bool>(); // Pool dictionary
    public Transform spawnPosition; // Position to spawn platforms
    public Transform tempPosition; // Temporary position holder
    
    void Start()
    {
        GameObject obj;
        
        for (int i = 0; i < poolSize; i++)
        {
            obj = Instantiate(objectPrefabs[Random.Range(0, objectPrefabs.Length)],
                              tempPosition.position,
                              Quaternion.identity); // Instantiate at temp position

            pool.Add(obj, true); // true indicates the object is available
        }

        for (int i = 0; i < startPrefabs; i++)
        {
            SpawnPrefab();
        }
    }

    void SpawnPrefab()
    {
        foreach (KeyValuePair<GameObject, bool> obj in pool)
        {
            if (obj.Value) // If the object is available
            {
                pool[obj.Key] = false; // Mark as in use
                obj.Key.transform.position = spawnPosition.position; // Move to spawn position
                NextSpawnPositionCal(obj.Key); // Calculate next spawn position
                StartCoroutine(ReturnPrefab(obj.Key)); // Start coroutine to return object to pool
                break; // Exit after spawning one object
            }
        }
    }

    void NextSpawnPositionCal(GameObject selPrefab)
    {
        spawnPosition.position = new Vector2(Random.Range(-2.8f, 2.8f),
                                             selPrefab.transform.position.y +
                                             Random.Range(2.1f, 2.75f)); // Update spawn position for next object
    }

    IEnumerator ReturnPrefab(GameObject selPrefab)
    {
        while (!selPrefab.GetComponent<Renderer>().isVisible)
        {
            yield return new WaitForEndOfFrame(); // Wait until the object is visible
        }
        while (selPrefab.GetComponent<Renderer>().isVisible)
        {
            yield return new WaitForEndOfFrame(); // Wait until the object is no longer visible
        }

        selPrefab.transform.position = tempPosition.position; // Move back to temp position
        pool[selPrefab] = true; // Mark as available

        SpawnPrefab(); // Spawn a new object
    }
}
