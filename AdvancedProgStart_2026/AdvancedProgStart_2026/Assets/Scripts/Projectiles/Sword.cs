using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private float LifeTime = 1f;
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
