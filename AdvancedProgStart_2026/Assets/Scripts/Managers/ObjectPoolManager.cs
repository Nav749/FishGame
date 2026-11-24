using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public enum PoolTypes
    {
        Bullet,
        Bomb
    }

    private static ObjectPoolManager instance;
    public static ObjectPoolManager Instance => instance;

    [SerializeField] private List<ObjectPool> objectPools;

    public int numObjecPools => objectPools.Count;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        GameEvents.OnGameReset += GameReset;
    }

    private void OnDisable()
    {
        GameEvents.OnGameReset -= GameReset;
    }

    private void Start()
    {
        GetComponentsInChildren<ObjectPool>(objectPools);
    }

    public GameObject GetPooledObject(PoolTypes poolType)
    {
        return objectPools[(int)poolType].GetPooledObject();
    }

    public void GameReset()
    {
        for(int i = 0; i < objectPools.Count; i++)
        {
            objectPools[i].GameReset();
        }
    }
}
