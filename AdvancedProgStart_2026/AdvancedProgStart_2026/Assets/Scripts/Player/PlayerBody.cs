using JetBrains.Annotations;
using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    #region Fields and Variables
    private Rigidbody rb;

    [SerializeField] private Transform fireLocation;
    [SerializeField] private GameObject turret;
    [HideInInspector] public Transform Turret { get { return turret.transform; } }

    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] Bulllet[] bullet;
    [SerializeField] Sword sword;
    [SerializeField] Grapple grapple;

    public bool activeGrapple = false;
    private Vector3 velocityToSet;

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
    }

    private void FixedUpdate()
    {
        if(!activeGrapple)
        {
            Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed * Time.fixedDeltaTime;

            rb.MovePosition(transform.position +  moveDir);
        }

        Debug.DrawRay(fireLocation.position, fireLocation.forward * 30, Color.red);
    }

    public void Swipe()
    {
        Sword s = sword;

        if(s != null)
        {
            s = Instantiate(s.gameObject, fireLocation.position, Quaternion.identity).GetComponent<Sword>();
            s.transform.rotation = turret.transform.rotation;
            s.gameObject.SetActive(true);
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
    }

    public void UpdateMove(Vector2 dir)
    {
        moveInput = dir;
    }

    public void Fire(Bulllet.BulletType type = Bulllet.BulletType.Default)
    {
        Vector3 dir = fireLocation.forward;
        
        dir.Normalize();

        Grapple g = grapple;

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
        rb.linearVelocity = velocityToSet;
    }
}
