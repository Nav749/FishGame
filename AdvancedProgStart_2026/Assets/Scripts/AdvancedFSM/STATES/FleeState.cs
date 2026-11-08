using System;
using UnityEngine;
using UnityEngine.AI;

public class FleeState : FSMState
{
    NavMeshAgent agent;
    Transform[] waypoints;
    Transform currentWaypoint;
    FleeProp fleeProp;
    Harpooner harpooner;

    public FleeState(Harpooner harp, FleeProp flee, NavMeshAgent a)
    {
        fleeProp = flee;
        harpooner = harp;

        stateID = StateID.Flee;
        waypoints = fleeProp.waypoints;
        curSpeed = fleeProp.speed;
        curRotSpeed = fleeProp.rotSpeed;

        agent = a;
    }

    public override void EnterState(Transform player, Transform npc)
    {
        agent.destination = GetNewWaypoint(player).position;
    }

    public override void CheckTransitions(Transform player, Transform npc)
    {
        if (harpooner.Health == 0)
        {
            harpooner.PerformTransition(TransitionID.NoHealth);
            return;
        }
        else if (Vector3.Distance(player.position, npc.position) > 50f)
        {
            harpooner.PerformTransition(TransitionID.FarEnough);
            return;
        }
    }

    public override void UpdateBehavior(Transform player, Transform npc)
    {
        if(currentWaypoint != null)
        {
        if(Vector3.Distance(currentWaypoint.position, npc.position) < 1f)
        {
            agent.destination = GetNewWaypoint(player).position;
        }

        }
    }

    Transform GetNewWaypoint(Transform player)
    {
        Transform newWayPoint;
        do
        {
            Array.Sort(waypoints, (a, b) =>
            {
                float distA = (a.position - player.position).sqrMagnitude;
                float distB = (b.position - player.position).sqrMagnitude;
                return distB.CompareTo(distA);
            });

            newWayPoint = waypoints[0];
        }
        while (newWayPoint == currentWaypoint);

        currentWaypoint = newWayPoint;
        return newWayPoint;
    }
}
