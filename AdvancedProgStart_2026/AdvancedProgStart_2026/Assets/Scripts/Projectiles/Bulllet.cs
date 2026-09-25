using UnityEngine;

public class Bulllet : MonoBehaviour
{
    public enum BulletType {Default, Grenade};

    public BulletType type = BulletType.Default;

    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float lifeTime = 5f;

    protected Vector3 direction = Vector3.forward;

    protected virtual void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void Fire(Vector3 dir)
    {
        direction = dir;
    }

    protected virtual void Update()
    {
        transform.Translate(speed * Time.deltaTime * direction);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
