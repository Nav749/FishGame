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

    private Grapple g;

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
        AddFSMState(chaseState);

        MeleeState meleeState = new MeleeState(this, meeleeProp);
        meleeState.AddTransition(TransitionID.NoHealth, StateID.Dead);
        meleeState.AddTransition(TransitionID.FarEnough, StateID.Chase);
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
        g.UpdateBody(rb);
        yield return new WaitForSeconds(time);
        hasShot = true;
    }

    public IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        shooting = false;
        hasShot = false;
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
}
