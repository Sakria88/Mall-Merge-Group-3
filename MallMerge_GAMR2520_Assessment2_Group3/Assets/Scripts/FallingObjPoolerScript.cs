using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjPoolerScript : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    public int poolSize = 10;
    public int startPrefabs = 3;

    public Transform spawnPosition;
    public Transform tempPosition;

    public float minSpawnDelay = 0.2f;
    public float maxSpawnDelay = 1f;

    private Dictionary<GameObject, bool> pool = new Dictionary<GameObject, bool>();

    void Start()
    {
        CreatePool();
        StartCoroutine(SpawnLoop());
    }

    void CreatePool()
    {
        int[] prefabCounts = { 4, 2, 1, 3 };
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            for (int j = 0; j < prefabCounts[i]; j++)
            {
                GameObject obj = Instantiate(objectPrefabs[i], tempPosition.position, Quaternion.identity);
                obj.SetActive(false);
                var falling = obj.GetComponent<FallingObjectScript>();
                if (falling != null) falling.OnReturnToPool += ReturnToPool;
                pool.Add(obj, true);
            }
        }
    }

    IEnumerator SpawnLoop()
    {
        while(true)
        {
            SpawnPrefab();
            float wait = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(wait);
        }
    }

    void SpawnPrefab()
    {
        List<GameObject> availableObjs = new List<GameObject>();
        foreach (var value in pool)
        {
            if (value.Value) availableObjs.Add(value.Key);
        }

        if (availableObjs.Count == 0) return;

        GameObject obj = availableObjs[Random.Range(0, availableObjs.Count)];
        pool[obj] = false;
        obj.transform.position = GetRandomSpawnPosition();
        obj.SetActive(true);
        StartCoroutine(ReturnPrefab(obj));
    }

    Vector2 GetRandomSpawnPosition()
    {
        Camera cam = Camera.main;

        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

        float margin = 0.5f;


        float minX = -camWidth / 2f + margin;
        float maxX = camWidth / 2f - margin;

        return new Vector2(
            Random.Range(minX, maxX),
            spawnPosition.position.y
        );
    }

    IEnumerator ReturnPrefab(GameObject selPrefab)
    {
        while (!selPrefab.GetComponent<Renderer>().isVisible)
        {
            yield return new WaitForEndOfFrame();
        }
        while (selPrefab.GetComponent<Renderer>().isVisible)
        {
            yield return new WaitForEndOfFrame();
        }
        ReturnToPool(selPrefab);
    }

    void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.position = tempPosition.position;
        pool[obj] = true;
    }
}
