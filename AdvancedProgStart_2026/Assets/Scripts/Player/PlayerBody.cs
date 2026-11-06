using System;
using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class PlayerBody : MonoBehaviour
{
    #region Fields and Variables
    [Header("References")]
    [SerializeField] private Transform fireLocation;
    [SerializeField] Sword sword;
    [SerializeField] Grapple grapple;
    [SerializeField] CinemachineCamera cam;

    [Space(10)]
    [Header("Speed")]
    [SerializeField] private float moveSpeed = 5f;

    [Space(10)]
    [Header("FOVs")]
    public float grappleFov = 95f;
    public float normalFov = 80f;

    [HideInInspector]public bool activeGrapple = false;
    private Rigidbody rb;
    private Vector3 velocityToSet;
    private Vector3 velocity;
    private Grapple g;

    [SerializeField] private GameObject turret;
    [HideInInspector] public Transform Turret { get { return turret.transform; } }

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
            //get the relative forward and right of the player and add the inputs
            Vector3 relativeForward = transform.forward * moveInput.y;
            Vector3 relativeRight = transform.right * moveInput.x;

            //create the relative angle to move
            Vector3 relativeAngle = relativeForward + relativeRight;

            //move the player by angle to move, move speed, the speedBoost, and fixed delta time
            Vector3 moveDir = new Vector3(relativeAngle.x, 0, relativeAngle.z) * moveSpeed * StatManager.Instance.SpeedBoost * Time.fixedDeltaTime;

            //Add movement to velocity to converse momentum
            rb.linearVelocity += moveDir;
        }
    }

    /// <summary>
    /// this spawns the hitbox for the sword and will eventually animate the sword
    /// </summary>
    public void Swipe()
    {
        //creates a reference to the sword
        Sword s = sword;

        if(s != null)
        {
            //spawns sword if null
            s = Instantiate(s.gameObject, fireLocation.position, Quaternion.identity).GetComponent<Sword>();
            s.transform.rotation = turret.transform.rotation;
            s.gameObject.SetActive(true);

            if(g != null)
            {
                //if the grapple exsists in the scene, stop it from pulling you and allow yourself to regrapple
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
        //normalizes the direction
        dir.Normalize();

        //sets the turret and fire location to the direction
        turret.transform.forward = dir;
        fireLocation.forward = dir;
        
        //rotates the rigidbody to face the direction
        Quaternion deltaRot = Quaternion.Euler(dir * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRot);
    }

    /// <summary>
    /// sets the move input direction
    /// </summary>
    /// <param name="dir">the input direction</param>
    public void UpdateMove(Vector2 dir)
    {
        moveInput = dir;
    }

    /// <summary>
    /// fires the grappling hook
    /// </summary>
    public void Fire()
    {
        //if the grappling hook is already out, dont launch a new one
        if (activeGrapple) return;

        //the direction of the grappling hook to be shot from normalized
        Vector3 dir = fireLocation.forward;
        dir.Normalize();

        //assigning the grapple to exsist
        g = grapple;

        if(g != null)
        {
            //if the grapple exsists, instatiate it
            g = Instantiate(g.gameObject, fireLocation.position, Quaternion.identity).GetComponent<Grapple>();
            g.gameObject.SetActive(true);
            g.UpdateDirection(dir);
            g.UpdateStartPoint(fireLocation);
            g.UpdateBody(this);
            g.StartGrapple();
        }
    }

    /// <summary>
    /// calculates the launch of the player when they are grappled
    /// </summary>
    /// <param name="startPoint"> the current location of the player</param>
    /// <param name="endPoint">the location where the grappling hook caught</param>
    /// <param name="trajectoryHeight">the amount of height the player goes above the highest point of the grapple</param>
    /// <returns> a vector 3 that holds the path of travel</returns>
    private Vector3 CalculateLaunch(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        //sets gravity
        float gravity = Physics.gravity.y;

        //calculates the displacements
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        //using the displacements, calculates the velocity
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        //returns the components as one vector
        return velocityXZ + velocityY;
    }

    /// <summary>
    /// sets the arc the player will travel on
    /// </summary>
    /// <param name="targetPosition"> the location where the grappling hook caught</param>
    /// <param name="trajectoryHeight"> the amount of hieght the player goes above the highest point of the grapple</param>
    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;

        //calls the calculate Lauch function
        velocityToSet = CalculateLaunch(transform.position, targetPosition, trajectoryHeight);

        Invoke("SetVelocity", 0.1f);
    }

    /// <summary>
    /// adds the velocity without losing momentum
    /// </summary>
    private void SetVelocity()
    {
        rb.linearVelocity += velocityToSet;
    }

    /// <summary>
    /// Changes the FOV of the camera
    /// </summary>
    /// <param name="goalFov">the fov that will be acheived</param>
    /// <param name="transitionTime">the time it will take to acheive that fov</param>
    /// <returns>a change in FOV over time</returns>
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
