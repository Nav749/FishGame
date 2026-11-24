using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance => instance;

    [SerializeField] private List<GameObject> enemyWaves;
    private Queue<GameObject> waves;

    private bool progressLevel;
    GameObject waveOfEnemies;

    private void Awake()
    {
        if (instance)
            Destroy(instance.gameObject);

        instance = this;

        progressLevel = false;
        waves = new Queue<GameObject>();
        foreach (GameObject enemy in enemyWaves)
        {
            waves.Enqueue(enemy);
        }
    }

    private void OnEnable()
    {
        GameEvents.OnGameReset += Reset;
    }

    private void OnDisable()
    {
        GameEvents.OnGameReset -= Reset;
    }

    private void Start()
    {
        spawnEnemyWave();
    }

    public void spawnEnemyWave()
    {
        if(waves.Count == 0)
        {
            progressLevel = true;
            return;
        }
        GameObject wave = waves.Dequeue();
        waveOfEnemies = Instantiate(wave, Vector3.zero, Quaternion.identity);
        WaveManager waveManager = waveOfEnemies.GetComponent<WaveManager>();
        foreach (GameObject enemy in waveManager.Enemies)
        {
            enemy.GetComponent<EnemyBase>().Agent.Warp(enemy.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!progressLevel)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            StatManager.Instance.ResetStats();
            UIManager.Instance.UpdatePickupCounter();
            GameManager.Instance.LevelComplete();
        }
    }

    private void Reset()
    {
        progressLevel=false;
        Destroy(waveOfEnemies);
        waves.Clear();
        waves = new Queue<GameObject>();
        foreach (GameObject enemy in enemyWaves)
        {
            waves.Enqueue(enemy);
        }
        spawnEnemyWave();
    }
}
