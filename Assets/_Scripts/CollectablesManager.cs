using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesManager : MonoBehaviour
{
    [SerializeField] List<PoolObject> pools;
    [SerializeField] Queue<GameObject> objectPool;

    public void InitializPool(int max)
    {
        int rand = 0;

        objectPool = new Queue<GameObject>();

        GameObject parent = new GameObject();

        parent.name = "Colletable Objects";

        for (int i = 0; i < max; i++)
        {
            rand = Random.Range(0, pools.Count - 1);

            GameObject obj = Instantiate(pools[rand].prefab);

            obj.SetActive(false);
            objectPool.Enqueue(obj);

            obj.transform.parent = parent.transform;
        }
    }

    public GameObject SpawnFromPool(Vector3 position, Quaternion rotation)
    {
        // If the pool is empty, instantiate a new object
        if (objectPool.Count == 0)
        {
            //int rand = Random.Range(0, pools.Count);

            //GameObject newObj = Instantiate(pools[rand].prefab);
            //return SetupPooledObject(newObj, position, rotation);

            return null;
        }

        // Otherwise, reuse an object from the pool
        GameObject objectToSpawn = objectPool.Dequeue();
        return SetupPooledObject(objectToSpawn, position, rotation);
    }

    private GameObject SetupPooledObject(GameObject obj, Vector3 position, Quaternion rotation)
    {
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        IPoolObject pooledObj = obj.GetComponent<IPoolObject>();
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        return obj;
    }

    public virtual void ReturnToPool(GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);
        objectPool.Enqueue(objectToReturn);
    }
}
