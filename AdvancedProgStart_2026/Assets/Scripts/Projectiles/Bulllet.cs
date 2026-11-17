using UnityEngine;
using System.Collections;

public class Bulllet : MonoBehaviour
{
    public enum BulletType {Default, Grenade};

    public BulletType type = BulletType.Default;

    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float lifeTime = 5f;

    protected Vector3 direction = Vector3.forward;

    public void Fire(Vector3 dir)
    {
        direction = dir;
    }

    protected virtual void FixedUpdate()
    {
        transform.Translate(speed * Time.fixedDeltaTime * direction);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        StartCoroutine(SelfDestruct());
    }

    public void OnDisable()
    {
        StopAllCoroutines();
    }

    protected virtual IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }
}
