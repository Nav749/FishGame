using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class MeleeProp: AIProp
{
    public GameObject hurtbox;
    public float waitTime;
}

[System.Serializable]
public class GrappleProp: AIProp
{
    public GameObject grapplePrefab;
    public float waitTime;
}

public class Fisher : FiniteStateMachine
{
    [SerializeField] private MeleeProp meeleeProp;
    [SerializeField] private AIProp chaseProp;
    [SerializeField] private GrappleProp grappleProp;
    [SerializeField] private float maxHealth;
    private float health;
    public float Health => health;
    [SerializeField] private GameObject playerObj;
    NavMeshAgent agent;
    [SerializeField] private Transform FireLocation;
    [SerializeField] private Rigidbody rb;

    [HideInInspector] public bool shooting = false;
    [HideInInspector] public bool hasShot = false;
    [SerializeField] private GameObject pickupPrefab;

    [SerializeField] TextMeshPro text;
    [SerializeField] float cooldown;

    private Vector3 velocityToSet;

    private Grapple g;
    public bool activeGrapple = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void DecHealth(float amount)
    {
        health = Mathf.Max(0, health - amount);
    }

    protected override void Initialize()
    {
        playerTransform = playerObj.transform;

        health = maxHealth;

        ConstructFSM();
    }

    private void ConstructFSM()
    {
        ChaseState chaseState = new ChaseState(this, chaseProp, agent);
        chaseState.AddTransition(TransitionID.NoHealth, StateID.Dead);
        chaseState.AddTransition(TransitionID.TooClose, StateID.Melee);
        chaseState.AddTransition(TransitionID.GreatDistance, StateID.Grapple);
        AddFSMState(chaseState);

        GrappleState grappleState = new GrappleState(this, grappleProp, rb);
        grappleState.AddTransition(TransitionID.NoHealth, StateID.Dead);
        grappleState.AddTransition(TransitionID.FarEnough, StateID.Chase);
        AddFSMState(grappleState);

        MeleeState meleeState = new MeleeState(this, meeleeProp);
        meleeState.AddTransition(TransitionID.NoHealth, StateID.Dead);
        meleeState.AddTransition(TransitionID.FarEnough, StateID.Chase);
        meleeState.AddTransition(TransitionID.GreatDistance, StateID.Grapple);
        AddFSMState(meleeState);

        DeadState deadState = new DeadState(this);
        AddFSMState(deadState);
    }

    protected override void FSMUpdate()
    {
        if (CurrentState != null)
        {
            CurrentState.CheckTransitions(playerTransform, this.transform);
            CurrentState.UpdateBehavior(playerTransform, this.transform);
        }

        text.text = CurrentState.ToString();
    }

    public void death()
    {
        GameObject pickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public IEnumerator FireBullet(float time)
    {
        shooting = true;
        GameObject s = Instantiate(meeleeProp.hurtbox, FireLocation.position, Quaternion.identity);
        Sword sword = s.GetComponent<Sword>();
        sword.user = User.Enemy;
        yield return new WaitForSeconds(time);
        shooting = false;
    }

    public IEnumerator FireBullet(float time, bool grapple)
    {
        shooting = true;
        GameObject s = Instantiate(grappleProp.grapplePrefab, FireLocation.position, Quaternion.identity);
        g = s.GetComponent<Grapple>();
        g.user = User.Enemy;
        Vector3 dir = FireLocation.forward;
        dir.Normalize();
        g.UpdateDirection(dir);
        g.UpdateStartPoint(FireLocation);
        g.UpdateBody(this);
        g.StartGrapple();
        yield return new WaitForSeconds(time);
        hasShot = true;
    }

    public IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        shooting = false;
        hasShot = false;
        Debug.Log("hfdjsaha");
    }

    public void LookatPlayer(Vector3 target, float speed)
    {
        Vector3 dir = target - transform.position;

        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, speed * Time.fixedDeltaTime);
        }
    }

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

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        //calls the calculate Lauch function
        velocityToSet = CalculateLaunch(transform.position, targetPosition, trajectoryHeight);

        Invoke("SetVelocity", 0.1f);
    }

    private void SetVelocity()
    {
        rb.linearVelocity += velocityToSet;
    }
}
