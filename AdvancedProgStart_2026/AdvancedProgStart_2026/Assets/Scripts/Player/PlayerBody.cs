using System;
using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class PlayerBody : MonoBehaviour
{
    #region Fields and Variables
    private Rigidbody rb;

    [SerializeField] private Transform fireLocation;
    [SerializeField] private GameObject turret;
    [HideInInspector] public Transform Turret { get { return turret.transform; } }

    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] Sword sword;
    [SerializeField] Grapple grapple;

    [SerializeField] CinemachineCamera cam;
    public float grappleFov = 95f;
    public float normalFov = 80f;

    public bool activeGrapple = false;
    private Vector3 velocityToSet;
    private Vector3 velocity;
    private Grapple g;

    Vector2 moveInput;
    public Vector2 MoveInout
    {
        get
        {
            return moveInput;
        }
    }

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        velocity = Vector3.zero;
    }

    private void Start()
    {
        cam.Lens.FieldOfView = normalFov;
    }

    private void FixedUpdate()
    {
        if(!activeGrapple)
        {
            Vector3 relativeForward = transform.forward * moveInput.y;
            Vector3 relativeRight = transform.right * moveInput.x;

            Vector3 relativeAngle = relativeForward + relativeRight;

            Vector3 moveDir = new Vector3(relativeAngle.x, 0, relativeAngle.z) * moveSpeed * StatManager.Instance.SpeedBoost * Time.fixedDeltaTime;

            rb.linearVelocity += moveDir;
        }
    }

    public void Swipe()
    {
        Sword s = sword;

        if(s != null)
        {
            s = Instantiate(s.gameObject, fireLocation.position, Quaternion.identity).GetComponent<Sword>();
            s.transform.rotation = turret.transform.rotation;
            s.gameObject.SetActive(true);
            if(g != null)
            {
                g.StopGrapple();
                activeGrapple = false;
            }
        }
    }
    /// <summary>
    /// updates the turret's aim direction
    /// </summary>
    /// <param name="dir"> a direction to aim to</param>
    public void UpdateTurret(Vector3 dir)
    {
        dir.Normalize();
        turret.transform.forward = dir;
        fireLocation.forward = dir;
        Quaternion deltaRot = Quaternion.Euler(dir * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRot);
    }

    public void UpdateMove(Vector2 dir)
    {
        moveInput = dir;
    }

    public void Fire(Bulllet.BulletType type = Bulllet.BulletType.Default)
    {
        if (activeGrapple) return;

        Vector3 dir = fireLocation.forward;
        
        dir.Normalize();

        g = grapple;

        if(g != null)
        {
            g = Instantiate(g.gameObject, fireLocation.position, Quaternion.identity).GetComponent<Grapple>();
            g.gameObject.SetActive(true);
            g.UpdateDirection(dir);
            g.UpdateStartPoint(fireLocation);
            g.UpdateBody(this);
            g.StartGrapple();
        }
    }

    private Vector3 CalculateLaunch(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;

        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;

        velocityToSet = CalculateLaunch(transform.position, targetPosition, trajectoryHeight);

        Invoke("SetVelocity", 0.1f);
    }

    private void SetVelocity()
    {
        rb.linearVelocity += velocityToSet;
    }

    public IEnumerator ChangeFov(float goalFov, float transitionTime)
    {
        float currFov = cam.Lens.FieldOfView;
        float time = 0f;

        while(time < transitionTime)
        {
            cam.Lens.FieldOfView = Mathf.Lerp(currFov, goalFov, time/transitionTime);
            time += Time.deltaTime;
            yield return null;
        }

        cam.Lens.FieldOfView = goalFov;
    }
}
