using NUnit.Compatibility;
using System.Collections;
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

        DeadState deadState = new DeadState();
        AddFSMState(deadState);
    }

    protected override void FSMUpdate()
    {
        if(CurrentState != null)
        {
            CurrentState.CheckTransitions(playerTransform, this.transform);
            CurrentState.UpdateBehavior(playerTransform, this.transform);
        }
    }

    public IEnumerator FireBullet()
    {
        GameObject b = Instantiate(attackProp.bullet, FireLocation.position, Quaternion.identity);
        yield return new WaitForSeconds(waitTime);
    }
}
