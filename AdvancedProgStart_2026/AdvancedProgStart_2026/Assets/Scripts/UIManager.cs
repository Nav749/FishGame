using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI pickupCounter;

    private float pickupCounterValue = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pickupCounter.text = pickupCounterValue.ToString();
    }

    public void UpdatePickupCounter()
    {
        pickupCounterValue += 1;
        pickupCounter.text = pickupCounterValue.ToString();
    }
}
