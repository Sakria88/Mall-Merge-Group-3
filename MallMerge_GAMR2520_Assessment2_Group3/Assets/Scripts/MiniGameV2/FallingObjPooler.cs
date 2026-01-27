using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FallingObjPooler : MonoBehaviour
{
    public GameObject[] prefabs;
    public Transform spawnPos;
    public Transform hidePos;

    public float minDelay = 0.2f;
    public float maxDelay = 1f;

    Dictionary<GameObject, bool> pool = new();

    void Start()
    {
        foreach (var p in prefabs)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject obj = Instantiate(p, hidePos.position, Quaternion.identity);
                obj.SetActive(false);
                obj.GetComponent<FallingObjectScriptV2>().OnReturnToPool += ReturnToPool;
                pool.Add(obj, true);
            }
        }

        StartCoroutine(SpawnLoop());
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
        foreach (var kv in pool)
        {
            if (kv.Value)
            {
                pool[kv.Key] = false;
                kv.Key.transform.position = GetRandomSpawnPosition();
                kv.Key.SetActive(true);
                return;
            }
        }
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

    void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.position = hidePos.position;
        pool[obj] = true;
    }
}
