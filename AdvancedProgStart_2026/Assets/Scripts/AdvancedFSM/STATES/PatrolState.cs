using System;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : FSMState
{
    NavMeshAgent agent;
    Transform[] waypoints;
    Transform currentWaypoint;
    int counter = 0;
    Flyer flyer;
    PatrolProp patrolProp;

    public PatrolState(Flyer fly, PatrolProp prop, NavMeshAgent a)
    {
        patrolProp = prop;
        flyer = fly;

        stateID = StateID.Patrol;
        waypoints = patrolProp.waypoints;
        curSpeed = patrolProp.speed;
        curRotSpeed = patrolProp.rotSpeed;

        agent = a;
        agent.baseOffset = 11.8f;
        currentWaypoint = GetNewWaypoint();
        agent.destination = currentWaypoint.position;
    }

    public override void CheckTransitions(Transform player, Transform npc)
    {
        if (flyer.Health == 0)
        {
            agent.ResetPath();
            flyer.PerformTransition(TransitionID.NoHealth);
            return;
        }
    }

    public override void UpdateBehavior(Transform player, Transform npc)
    {
        if (currentWaypoint != null)
        {
            if (Vector3.Distance(currentWaypoint.position, new Vector3(npc.position.x, currentWaypoint.position.y, npc.position.z)) < 1f)
            {
                currentWaypoint = GetNewWaypoint();
                agent.destination = currentWaypoint.position;
            }
        }
        Debug.Log(currentWaypoint.position);
    }

    private Transform GetNewWaypoint()
    {
        Transform newWaypoint;

        if (counter >= waypoints.Length)
        {
            counter = 0;
        }

        newWaypoint = waypoints[counter];
        counter++;
        return newWaypoint;
    }
}
