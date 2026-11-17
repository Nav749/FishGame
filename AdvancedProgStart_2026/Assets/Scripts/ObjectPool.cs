using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject pooledObject;
    [SerializeField] private int pooledAmount;
    [SerializeField] private bool willGrow = true;

    public List<GameObject> pooledObjects;

    void Start()
    {
        pooledObjects = new List<GameObject>();

        for(int i = 0; i < pooledAmount; i++)
        {
            CreatePooledObject();
        }
    }

    private GameObject CreatePooledObject()
    {
        GameObject obj = Instantiate(pooledObject, transform);
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }

    public GameObject GetPooledObject()
    {
        for(int i = 0;i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
                return pooledObjects[i];
        }
        if (willGrow)
            return CreatePooledObject();

        return null;
    }

    public void GameReset()
    {
        for(int i = 0; i< pooledObjects.Count; i++)
        {
            pooledObjects[i].SetActive(false);
        }
    }
}
