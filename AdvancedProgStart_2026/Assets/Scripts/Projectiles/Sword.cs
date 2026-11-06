using UnityEngine;

public class Sword : MonoBehaviour
{
    [Header("Values")]
    [Range(0.01f, 1f), Tooltip("Life time of the hitbox")]
    [SerializeField] private float LifeTime = 1f;
    [Range(30f, 100f), Tooltip("Amount of Force applied to Pushable Objects")]
    [SerializeField] private float force = 50f;

    private void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pushable"))
        {
            other.GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Impulse);
        }
    }
}
