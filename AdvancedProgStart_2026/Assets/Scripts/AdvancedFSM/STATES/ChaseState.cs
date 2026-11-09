using UnityEngine;
using UnityEngine.AI;

public class ChaseState : FSMState
{
    Fisher fisher;
    AIProp aIProp;
    NavMeshAgent agent;
    public ChaseState(Fisher f, AIProp chase, NavMeshAgent a)
    {
        fisher = f;
        aIProp = chase;

        stateID = StateID.Chase;
        curSpeed = aIProp.speed;
        curRotSpeed = aIProp.rotSpeed;
        agent = a;
    }

    public override void CheckTransitions(Transform player, Transform npc)
    {
        if(fisher.Health == 0)
        {
            agent.ResetPath();
            fisher.PerformTransition(TransitionID.NoHealth);
            return;
        }
        else if (Vector3.Distance(player.position, npc.position) < 2f)
        {
            agent.ResetPath();
            fisher.PerformTransition(TransitionID.TooClose);
            return;
        }
    }

    public override void UpdateBehavior(Transform player, Transform npc)
    {
        float xVal = Random.Range(-5f, 5f);
        float zVal = Random.Range(-5f, 5f);
        Vector3 dest = new Vector3(player.position.x + xVal, player.position.y, player.position.z + zVal);
        agent.destination = dest;
    }
}
