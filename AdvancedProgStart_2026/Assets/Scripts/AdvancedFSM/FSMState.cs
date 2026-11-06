using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class represents the States in the Finite State System.
/// Each state has a Dictionary with pairs (transition-state) showing
/// which state the FSM should be if a transition is fired
/// CheckTransitions method is used to determine which transition should be fired
/// </summary>
public abstract class FSMState
{
    protected Dictionary<TransitionID, StateID> map = new Dictionary<TransitionID, StateID>();
    protected StateID stateID;
    public StateID ID { get { return stateID; } }
    protected Vector3 destPos;
    protected Transform[] waypoints;
    protected float curRotSpeed;
    protected float curSpeed;

    public void AddTransition(TransitionID transition, StateID id)
    {
        // Check if anyone of the args is invallid
        if (transition == TransitionID.None || id == StateID.None)
        {
            Debug.LogWarning("FSMState : Null transition not allowed");
            return;
        }

        //Check if the current transition was already inside the map
        if (map.ContainsKey(transition))
        {
            Debug.LogWarning($"FSMState ERROR: transition {transition} is already inside the map");
            return;
        }

        map.Add(transition, id);
        Debug.Log($"Added : {transition} with ID : {id}");
    }

    /// <summary>
    /// This method deletes a pair transition-state from this state�s map.
    /// If the transition was not inside the state�s map, an ERROR message is printed.
    /// </summary>
    public void DeleteTransition(TransitionID trans)
    {
        // Check for NullTransition
        if (trans == TransitionID.None)
        {
            Debug.LogError($"FSMState ERROR: NullTransition {trans} is not allowed");
            return;
        }

        // Check if the pair is inside the map before deleting
        if (map.ContainsKey(trans))
        {
            map.Remove(trans);
            return;
        }
        Debug.LogError($"FSMState ERROR: Transition {trans} passed was not on this States List");
    }

    /// <summary>
    /// This method returns the new state the FSM should be if
    /// this state receives a transition  
    /// </summary>
    public StateID GetOutputState(TransitionID trans)
    {
        // Check for NullTransition
        if (trans == TransitionID.None)
        {
            Debug.LogError($"FSMState ERROR: NullTransition {trans} is not allowed");
            return StateID.None;
        }

        // Check if the map has this transition
        if (map.ContainsKey(trans))
        {
            return map[trans];
        }

        Debug.LogError($"FSMState ERROR: {trans} Transition passed to the State was not on the list");
        return StateID.None;
    }

    /// <summary>
    /// Used to initialize variables when re-entering state
    /// </summary>
    public virtual void EnterState(Transform player, Transform npc){}

    /// <summary>
    /// Decides if the state should transition to another on its list
    /// NPC is a reference to the npc that is controlled by this class
    /// </summary>
    public abstract void CheckTransitions(Transform player, Transform npc);

    /// <summary>
    /// This method controls the behavior of the NPC in the game World.
    /// Every action, movement or communication the NPC does should be placed here
    /// NPC is a reference to the npc that is controlled by this class
    /// </summary>
    public abstract void UpdateBehavior(Transform player, Transform npc);

    /// <summary>
    /// Find the next point
    /// </summary>
    public virtual void FindNextPoint()
    {
        int randomIndex = Random.Range(0, waypoints.Length);
        destPos = waypoints[randomIndex].position;
    }

    public Transform GetFurthestWayPoint(Transform trans)
    {
        return GetWayPoint(trans, true);
    }

    public Transform GetClosestWaypoint(Transform trans)
    {
        return GetWayPoint(trans);
    }

    /// <summary>
    /// Check whether the next random position is the same as current position
    /// </summary>
    /// <param name="pos">position to check</param>
    protected virtual bool IsInCurrentRange(Transform trans, Vector3 pos, float range)
    {
        bool inRange = false;
        float dist = Vector3.Distance(trans.position, pos);
        if (dist <= range)
        {
            inRange = true;
        }
        return inRange;
    }

    private Transform GetWayPoint(Transform trans, bool furthest = false)
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            return null;
        }

        float currSqrMagnitude = (trans.position - waypoints[0].position).sqrMagnitude;
        Transform retValue = waypoints[0];

        for (int i = 1; i < waypoints.Length; i++)
        {
            float sqrMagnitude = (trans.position - waypoints[i].position).sqrMagnitude;

            if ((furthest && (sqrMagnitude > currSqrMagnitude)) || (!furthest && (sqrMagnitude < currSqrMagnitude)))
            {
               retValue = waypoints[i];
               currSqrMagnitude = sqrMagnitude;
            }
        }

        return retValue;
    }


}
