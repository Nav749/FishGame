using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance => instance;

    [SerializeField] private List<GameObject> enemyWaves;
    private Queue<GameObject> waves;

    private bool progressLevel;

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
        spawnEnemyWave();
    }

    private void spawnEnemyWave()
    {
        if(waves.Count < 0)
        {
            progressLevel = true;
            return;
        }
        GameObject wave = waves.Dequeue();
        GameObject waveOfEnemies = Instantiate(wave, Vector3.zero, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!progressLevel)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            GameManager.Instance.LevelComplete();
        }
    }
}
