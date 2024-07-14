using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab; 
    public int initialPoolSize = 10; 
    public int maxPoolSize = 50; 

    private List<GameObject> pool;
    private int nextAvailableIndex = 0;

    void Start()
    {
      
        pool = new List<GameObject>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            AddNewObjectToPool();
        }
    }

    public GameObject Get()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            int index = (nextAvailableIndex + i) % pool.Count;
            if (!pool[index].activeInHierarchy)
            {
                nextAvailableIndex = (index + 1) % pool.Count;
                pool[index].SetActive(true);
                return pool[index];
            }
        }

     
        if (pool.Count < maxPoolSize)
        {
            return AddNewObjectToPool();
        }

    
        return null;
    }


    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    private GameObject AddNewObjectToPool()
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        pool.Add(obj);
        return obj;
    }
}
