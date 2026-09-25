using System.Collections;
using UnityEngine;

public class Grenade : Bulllet
{
    private Rigidbody rb;
    [SerializeField] float explosionSize = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {

    }

    private void FixedUpdate()
    {
        if(rb != null)
        {
            Vector3 moveDir = direction * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position +  moveDir);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            direction = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = true;
            StartCoroutine(KaBoom());
        }
    }

    IEnumerator KaBoom()
    {
        Vector3 origScale = transform.localScale;
        float currScale = origScale.x;

        while(currScale < explosionSize)
        {
            currScale = Mathf.MoveTowards(currScale, explosionSize, Time.deltaTime);

            transform.localScale = Vector3.one * currScale;

            yield return null;
        }
    }
}
