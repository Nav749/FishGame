using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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
            agent.ResetPath();
            harpooner.PerformTransition(TransitionID.NoHealth);
            return;
        }
        else if (Vector3.Distance(player.position, npc.position) > 30f)
        {
            agent.ResetPath();
            harpooner.PerformTransition(TransitionID.FarEnough);
            return;
        }
    }

    public override void UpdateBehavior(Transform player, Transform npc)
    {
        if (currentWaypoint != null)
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
            Array.Sort(waypoints, (a, b) =>
            {
                float distA = (a.position - player.position).sqrMagnitude;
                float distB = (b.position - player.position).sqrMagnitude;
                return distB.CompareTo(distA);
            });

            float rnd = Random.Range(0.0f, 1.0f);

        if (rnd < 0.9f) newWayPoint = waypoints[0];
        else if(rnd < 0.99f) newWayPoint= waypoints[1];
        else newWayPoint = waypoints[2];

        if(currentWaypoint == newWayPoint)
        {
            newWayPoint = waypoints[3];
        }

        currentWaypoint = newWayPoint;
        return newWayPoint;
    }
}
