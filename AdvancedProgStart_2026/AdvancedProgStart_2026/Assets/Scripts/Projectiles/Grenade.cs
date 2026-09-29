using System;
using System.Collections;
using UnityEngine;

public class Grenade : Bulllet
{
    private Rigidbody rb;

    [Space(10f)]
    [Header("Explosion Variables")]

    [Range(1f, 100000000000f)]
    [SerializeField] float explosionSize = 5f;

    [Tooltip("Leave this at 1.0 to use deltatime")]
    [SerializeField] float expandSpeed = 1f;

    [Space(10f)]
    [Header("Fade Variables")]
    [SerializeField] float fadeAmount = 0.1f;
    [SerializeField] float fadeWait = 0.01f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        if (rb != null)
        {
            Vector3 moveDir = direction * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveDir);
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

        while (currScale < explosionSize)
        {
            currScale = Mathf.MoveTowards(currScale, explosionSize, Time.deltaTime * expandSpeed);

            transform.localScale = Vector3.one * currScale;

            yield return null;
        }

        yield return StartCoroutine(Fade(fadeAmount, fadeWait));
    }

    IEnumerator Fade(float fadeAmount, float fadeWait)
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer)
        {
            float alpha = renderer.material.color.a;

            for(; alpha > 0;  alpha-= fadeAmount)
            {
                Color c = renderer.material.color;
                c.a = alpha;
                renderer.material.color = c;

                yield return new WaitForSeconds(fadeWait);
            }

            Destroy(gameObject);
        }
    }
}
