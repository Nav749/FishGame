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
        FleeState fleeState = new FleeState(this, fleeProp, agent);
        fleeState.AddTransition(TransitionID.NoHealth, StateID.Dead);
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
}
