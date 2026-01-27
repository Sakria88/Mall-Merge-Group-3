using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FallingObjPooler : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    public int[] prefabAmounts;
    public Transform spawnPos;
    public Transform tempPos;

    public int poolSize = 10;
    public int startPrefabs = 3;

    public float minDelay = 0.2f;
    public float maxDelay = 1f;

    Dictionary<GameObject, bool> pool = new();

    void Start()
    {
        CreatePool();
        StartCoroutine(SpawnLoop());
    }

    void CreatePool()
    {
        if (prefabAmounts.Length != objectPrefabs.Length)
        {
            Debug.LogError("Prefab amounts length does not match object prefabs length.");
            return;
        } else
        {
            for (int i = 0; i < objectPrefabs.Length; i++)
            {
                for (int j = 0; j < prefabAmounts[i]; j++)
                {
                    GameObject obj = Instantiate(objectPrefabs[i], tempPos.position, Quaternion.identity);
                    obj.SetActive(false);
                    var falling = obj.GetComponent<FallingObjectScript>();
                    if (falling != null) falling.OnReturnToPool += ReturnToPool;
                    pool.Add(obj, true);
                }
            }
            return;
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }

    void Spawn()
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
            spawnPos.position.y
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
        obj.transform.position = tempPos.position;
        pool[obj] = true;
    }
}
