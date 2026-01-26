using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages reusable item objects so we don't Instantiate/Destroy during play
/// </summary>
public class ItemPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public ChestFamily family;   // Which family this pool is for
        public GameObject prefab;    // UI prefab with Image + MergeItem
        public int preload = 15;     // How many to create at start
    }

    public List<Pool> pools = new List<Pool>();

    private Dictionary<ChestFamily, Queue<GameObject>> poolDict = new();
    private Dictionary<ChestFamily, GameObject> prefabDict = new();

    private void Awake()
    {
        foreach (var p in pools)
        {
            prefabDict[p.family] = p.prefab;

            Queue<GameObject> q = new Queue<GameObject>();
            poolDict[p.family] = q;

            // Pre-create objects
            for (int i = 0; i < p.preload; i++)
            {
                GameObject obj = Instantiate(p.prefab, transform);
                obj.SetActive(false);
                q.Enqueue(obj);
            }
        }
    }

    /// <summary>
    /// Gets an item from the pool
    /// </summary>
    public GameObject Get(ChestFamily family)
    {
        if (!poolDict.ContainsKey(family))
        {
            Debug.LogError("No pool for " + family);
            return null;
        }

        Queue<GameObject> q = poolDict[family];

        if (q.Count > 0)
        {
            GameObject obj = q.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        // Pool empty → create a new one
        GameObject created = Instantiate(prefabDict[family], transform);
        created.SetActive(true);
        return created;
    }

    /// <summary>
    /// Returns an item back into the pool
    /// </summary>
    public void Return(ChestFamily family, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform, false);
        poolDict[family].Enqueue(obj);
    }
}
