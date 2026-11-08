using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
    public class AIProp
    {
        public float speed = 3f;
        public float rotSpeed = 2f;
    }

[System.Serializable]
public class FleeProp: AIProp
{
     public Transform[] waypoints;
}

[System.Serializable]
public class AttackProp: AIProp
{
    public GameObject bullet;
}

public class Harpooner : FiniteStateMachine
{
    [SerializeField] FleeProp fleeProp;
    [SerializeField] AttackProp attackProp;

    [SerializeField] private float maxHealth;
    private float health;
    public float Health => health;
    [SerializeField] private GameObject playerObj;
    NavMeshAgent agent;

    [SerializeField] private float waitTime = 2f;
    [SerializeField] private Transform FireLocation;

    [HideInInspector] public bool shooting;
    [SerializeField] private GameObject pickupPrefab;

    [SerializeField] TextMeshPro text;

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
        RangedAttackState attacktate = new RangedAttackState(this, attackProp);
        attacktate.AddTransition(TransitionID.NoHealth, StateID.Dead);
        attacktate.AddTransition(TransitionID.TooClose, StateID.Flee);
        AddFSMState(attacktate);

        FleeState fleeState = new FleeState(this, fleeProp, agent);
        fleeState.AddTransition(TransitionID.NoHealth, StateID.Dead);
        fleeState.AddTransition(TransitionID.FarEnough, StateID.Attack);
        AddFSMState(fleeState);

        DeadState deadState = new DeadState(this);
        AddFSMState(deadState);
    }

    protected override void FSMUpdate()
    {
        if(CurrentState != null)
        {
            CurrentState.CheckTransitions(playerTransform, this.transform);
            CurrentState.UpdateBehavior(playerTransform, this.transform);
        }

        text.text = CurrentState.ToString();
    }

    public IEnumerator FireBullet()
    {
        shooting = true;
        GameObject b = Instantiate(attackProp.bullet, FireLocation.position, Quaternion.identity);
        Bulllet bull = b.GetComponent<Bulllet>();
        bull.Fire(this.transform.forward);
        yield return new WaitForSeconds(waitTime);
        shooting = false;
    }

    public void LookatPlayer(Vector3 target, float speed)
    {
        Vector3 dir = target - transform.position;

        if(dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, speed * Time.fixedDeltaTime);
        }
    }

    public void death()
    {
        GameObject pickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
