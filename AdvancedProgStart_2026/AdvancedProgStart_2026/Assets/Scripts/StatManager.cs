using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance { get; private set; }

    private float speedBoost = 1f;
    private float speedIncrements = 0f;

    public float SpeedBoost
    {
        get
        {
            return speedBoost;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    public void IncrementSpeedStat()
    {
        speedIncrements++;

        speedBoost = 150f * (speedIncrements / (speedIncrements + 100f)); 
    }
}
