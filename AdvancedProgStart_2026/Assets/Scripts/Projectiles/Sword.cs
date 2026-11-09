using UnityEngine;

public enum User
{
    Player,
    Enemy
}

public class Sword : MonoBehaviour
{
    [Header("Values")]
    [Range(0.01f, 1f), Tooltip("Life time of the hitbox")]
    [SerializeField] private float LifeTime = 1f;
    [Range(30f, 100f), Tooltip("Amount of Force applied to Pushable Objects")]
    [SerializeField] private float force = 50f;
    [SerializeField] private float damage = 40f;

    [HideInInspector] public User user = User.Player;

    private void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(user == User.Player)
        {
            if (other.CompareTag("Pushable"))
            {
                other.GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Impulse);
            }
            else if (other.CompareTag("Enemy"))
            {
                Harpooner h;
                Flyer f;
                Fisher fish;
                if (other.TryGetComponent<Harpooner>(out h))
                {
                    h.DecHealth(damage);
                }
                else if (other.TryGetComponent<Flyer>(out f))
                {
                    f.DecHealth(damage);
                }
                else if (other.TryGetComponent<Fisher>(out fish))
                {
                    fish.DecHealth(damage);
                }
            }
        }
        else if(user == User.Enemy)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Rigidbody>().AddForce(transform.forward * (force / 4), ForceMode.Impulse);
            }
        }
    }
}
