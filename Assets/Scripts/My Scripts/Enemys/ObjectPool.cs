using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    public GameObject[] enemyPrefabs; // Prefab array to pool
    public int poolSize = 3; // Initial pool size

    private List<GameObject> pool;

    void Awake()
    {
        instance = this;
        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            // Create pool entries for each prefab type
            foreach (GameObject prefab in enemyPrefabs)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        // If no inactive object is available, instantiate a new one from a random prefab
        int prefabIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject newObj = Instantiate(enemyPrefabs[prefabIndex]);
        newObj.SetActive(false);
        pool.Add(newObj);
        return newObj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
