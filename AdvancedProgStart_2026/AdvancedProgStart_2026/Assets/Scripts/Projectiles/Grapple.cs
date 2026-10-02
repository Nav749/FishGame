using UnityEngine;

public class Grapple : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask Grappleable;

    private Transform originPoint;
    private Vector3 direction;
    private LineRenderer lr;
    private PlayerBody body;

    [Header("Grappling")]
    [SerializeField] private float grappleDistance;
    [SerializeField] private float grappleDelay;
    [SerializeField] private float overshootYAxis;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    [SerializeField] private float grapplingCd;

    private float grapplingCdTimer;

    private bool grappling;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if(grapplingCdTimer > 0){
            grapplingCdTimer -= Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        if(grappling) lr.SetPosition(0, originPoint.position);
    }

    public void UpdateDirection(Vector3 dir)
    {
        direction = dir;
    }

    public void UpdateStartPoint(Transform t)
    {
        originPoint = t;
    }

    public void UpdateBody(PlayerBody b)
    {
        body = b;
    }

    public void StartGrapple()
    {
        if(grapplingCdTimer > 0) return;

        grappling = true;

        StopAllCoroutines();

        RaycastHit hit;
        if(Physics.Raycast(originPoint.position, direction, out hit, grappleDistance, Grappleable))
        {
            grapplePoint = hit.point;

            StartCoroutine(body.ChangeFov(body.grappleFov, grappleDelay));

            Invoke("ExecuteGrapple", grappleDelay);
        }
        else
        {
            grapplePoint = originPoint.position + direction * grappleDistance;

            Invoke("StopGrapple", grappleDelay);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple()
    {
        Transform t = body.gameObject.transform;

        Vector3 lowestPoint = new Vector3(t.position.x, t.position.y - 1f, t.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if(grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        body.JumpToPosition(grapplePoint, highestPointOnArc);

        StartCoroutine(body.ChangeFov(body.normalFov, 1f));

        Invoke("StopGrapple", 1f);
    }

    private void StopGrapple()
    {
        grappling = false;

        grapplingCdTimer = grapplingCd;

        lr.enabled = false;

        body.activeGrapple = false;

        Destroy(this.gameObject);
    }
}
