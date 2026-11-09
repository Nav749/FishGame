using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class MeeleeProp: AIProp
{
    public GameObject hurtbox;
}

public class Fisher : FiniteStateMachine
{
    [SerializeField] private MeeleeProp meeleeProp;
    [SerializeField] private float maxHealth;
    private float health;
    public float Health => health;
    [SerializeField] private GameObject playerObj;
    NavMeshAgent agent;

    [SerializeField] private float waitTime = 2f;
    [SerializeField] private Transform FireLocation;

    [HideInInspector] public bool shooting = false;
    [HideInInspector] public bool hasShot = false;
    [SerializeField] private GameObject pickupPrefab;

    [SerializeField] TextMeshPro text;
    [SerializeField] float cooldown;

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
        GameObject s = Instantiate(meeleeProp.hurtbox, FireLocation.position, Quaternion.identity);
        Sword sword = s.GetComponent<Sword>();
        yield return new WaitForSeconds(waitTime);
        hasShot = true;
    }

    public IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        shooting = false;
        hasShot = false;
    }
}
