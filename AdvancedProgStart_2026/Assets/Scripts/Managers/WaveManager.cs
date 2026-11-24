using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private static WaveManager instance;
    public static WaveManager Instance;

    [SerializeField] private List<GameObject> enemies;
    public List<GameObject> Enemies => enemies;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        CheckForEndOfWave();
    }

    private void CheckForEndOfWave()
    {
        if (enemies == null || enemies.All(item => item == null))
        {
            LevelManager.Instance.spawnEnemyWave();
            Destroy(gameObject);
        }
    }
}
