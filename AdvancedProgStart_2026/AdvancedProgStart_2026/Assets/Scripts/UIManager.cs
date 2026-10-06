using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //Quick and Dirty Singleton pt2, the electric boogaloo
    public static UIManager Instance { get; private set; }

    [Tooltip("The textfield to update the pickup counter")]
    [SerializeField] TextMeshProUGUI pickupCounter;

    //the amount of pickups already picked up
    private float pickupCounterValue = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pickupCounter.text = pickupCounterValue.ToString();
    }

    /// <summary>
    /// updates the pickups and the textfield
    /// </summary>
    public void UpdatePickupCounter()
    {
        pickupCounterValue += 1;
        pickupCounter.text = pickupCounterValue.ToString();
    }
}
