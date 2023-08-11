using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler: MonoBehaviour {
    [System.Serializable]
    public class Pool {
        public string tag;
        public GameObject prefab;
        public int size;
        public Transform parent;
    }
    public static ObjectPooler Instance;
    private void Awake()
    {
        Instance = this;
        InitializeObjectPool();
    }
    //ı

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    private void InitializeObjectPool()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        for(int i = 0; i < pools.Count; i++)
        {
            Pool pool = pools[i];
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int a = 0; a < pool.size; a++)
            {
                GameObject obj = Instantiate(pool.prefab, pool.parent);
                obj.SetActive(false);
                if(obj.TryGetComponent (out PooledObject pooledObj))
                { 
                    pooledObj.InitializeId(a);
                }
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }

    }
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool with tag " + tag + " doesn't exist.");
            return null;
        }
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.transform.localScale = scale;
        objectToSpawn.SetActive(true);
        poolDictionary[tag].Enqueue(objectToSpawn);
        if(objectToSpawn.TryGetComponent (out PooledObject pooledObj))
        {
            pooledObj.OnObjectSpawn();
        }
        return objectToSpawn;

    }
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation )
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool with tag " + tag + " doesn't exist.");
            return null;
        }
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation; 
        objectToSpawn.SetActive(true);
        poolDictionary[tag].Enqueue(objectToSpawn);
        if(objectToSpawn.TryGetComponent(out PooledObject pooledObj))
        {
            pooledObj.OnObjectSpawn();
        }
        return objectToSpawn;

    }
    public GameObject GetPoolObjectPrefab(string tag)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool with tag " + tag + " doesn't exist.");
            return null;
        } 
        for(int i = 0; i < pools.Count; i++)
        {
            if(pools[i].tag == tag)
            {
                return pools[i].prefab;
            }
        }
        return null;
    }
    public GameObject GetPoolObject(string tag,int id)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject[] queueObjects = poolDictionary[tag].ToArray();
        for(int i = 0; i < queueObjects.Length; i++)
        { 
            if(queueObjects[i].TryGetComponent(out PooledObject pooledObj))
            {
                if(pooledObj.stackId == id)
                {
                    return queueObjects[i];
                }
            }
        }
        return null;
    }
}
