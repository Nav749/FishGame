using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class PatrolProp : AIProp
{
    public Transform[] waypoints;
}

[System.Serializable]
public class BombProp : AIProp
{
    public GameObject bullet;
}

public class Flyer : FiniteStateMachine
{
    [SerializeField] PatrolProp patrolProp;
    [SerializeField] BombProp bombProp;

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
        PatrolState patrolState = new PatrolState(this, patrolProp, agent);
        patrolState.AddTransition(TransitionID.NoHealth, StateID.Dead);
        AddFSMState(patrolState);

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

    public IEnumerator FireBullet()
    {
        shooting = true;
        GameObject b = Instantiate(bombProp.bullet, FireLocation.position, Quaternion.identity);
        Bulllet bull = b.GetComponent<Bulllet>();
        bull.Fire(this.transform.forward);
        yield return new WaitForSeconds(waitTime);
        shooting = false;
    }
}