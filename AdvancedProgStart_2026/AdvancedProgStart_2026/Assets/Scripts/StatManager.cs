using UnityEngine;

public class StatManager : MonoBehaviour
{
    //Quick and Dirty Singleton pt 3, indubitably
    public static StatManager Instance { get; private set; }

    //Speed variables
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

    /// <summary>
    /// Increments the speed boost stat in accordance to a function. Approaches 150%, hits 50% after 50 pickups.
    /// </summary>
    public void IncrementSpeedStat()
    {
        speedIncrements++;

        speedBoost = 150f * (speedIncrements / (speedIncrements + 100f)); 
    }
}
