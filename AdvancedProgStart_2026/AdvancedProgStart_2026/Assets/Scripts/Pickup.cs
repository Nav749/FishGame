using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Range(30f, 120f), Tooltip("Sets the Rotation speed of the Pickup")]
    [SerializeField] float rotationSpeed;

    private void Update()
    {
        //Rotate the Pickup
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnDestroy()
    {
        //Update the UI and Stats before Destruction
        UIManager.Instance.UpdatePickupCounter();
        StatManager.Instance.IncrementSpeedStat();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if its the player and destroy oneself
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
