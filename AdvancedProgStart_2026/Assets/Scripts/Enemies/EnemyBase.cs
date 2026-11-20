using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : FiniteStateMachine
{
    [SerializeField] protected float maxHealth;
    protected float health;
    public float Health => health;
    protected GameObject playerObj;
    protected NavMeshAgent agent;
    public NavMeshAgent Agent => agent;

    protected override void Initialize()
    {
    }

    protected override void FSMUpdate()
    {
    }
}
