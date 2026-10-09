using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This FSM class should be inherrited by any class that needs to use a FSM
/// to manage the states of the class and the transitions between them.
/// 
/// This FSM Implementation may be considered creative commons
/// You may use, modify and distribute the code in any projects.
/// </summary>

public enum TransitionID
{
    None = 0,
}

public enum StateID
{
    None = 0,
}

public abstract class FiniteStateMachine : MonoBehaviour
{
    #region private fields
    private List<FSMState> fsmStates;
    private StateID currentStateID;
    private FSMState currentState;
    #endregion

    #region public fields
    protected Transform playerTransform;

    //The fsmStates are not changing directly but updated by using transitions
    //therefore we have no setters available only getters
    public StateID CurrentStateID { get { return currentStateID; } }

    public FSMState CurrentState { get { return currentState; } }
    #endregion


    #region Unity methods
    void Awake()
    {
        fsmStates = new List<FSMState>();
    }
    void Start()
    {
        Initialize();
    }

    void Update()
    {
        FSMUpdate();
    }

    void FixedUpdate()
    {
        FSMFixedUpdate();
    }
    #endregion

    #region private methods
    protected abstract void Initialize();
    protected abstract void FSMUpdate();
    protected virtual void FSMFixedUpdate() { }
    #endregion

    #region public methods
    /// <summary>
    /// Adds a new State into the list if it's not already in the list
    /// </summary>
    public void AddFSMState(FSMState fsmState)
    {
        // check if the state is null
        if (fsmState == null)
        {
            Debug.LogError($"FSM ERROR | AddFSMState() | The state passed is null");
            return;
        }

        // if no state in the current then add the state to the list
        if (fsmStates.Count == 0)
        {
            fsmStates.Add(fsmState);
            currentState = fsmState;
            currentStateID = fsmState.ID;
            return;
        }

        // check if the state is already in the list
        foreach (FSMState state in fsmStates)
        {
            if (state.ID == fsmState.ID)
            {
                Debug.LogError($"FSM ERROR | AddFSMState() | {fsmState.ID} was already in the list");
                return;
            }
        }

        // If the state is not in the list then add it
        fsmStates.Add(fsmState);
    }

    /// <summary>
    /// Deletes a state from the FSM List if it exists
    /// </summary>
    public void DeleteState(StateID stateID)
    {
        // check if the state is null
        if (stateID == StateID.None)
        {
            Debug.LogError($"FSM ERROR | DeleteState() | None id is not allowed");
            return;
        }

        // search the List and remove the state if it's in the list
        foreach (FSMState state in fsmStates)
        {
            if (state.ID == stateID)
            {
                fsmStates.Remove(state);
                return;
            }
        }
        Debug.LogError($"FSM ERROR | DeleteState() | The state {stateID} was not in the list");
    }

    /// <summary>
    /// Tries to change the state of the FSM based on the transition passed
    /// </summary>
    public void PerformTransition(TransitionID transitionID)
    {
        // check if the transition is null
        if (transitionID == TransitionID.None)
        {
            Debug.LogError($"FSM ERROR | PerformTransition() | {transitionID} transition is not allowed");
            return;
        }

        StateID id = currentState.GetOutputState(transitionID);
        if (id == StateID.None)
        {
            Debug.LogError("FSM ERROR: Current State does not have a target state for this transition");
            return;
        }

        // update the currentStateID and currentState
        currentStateID = id;
        foreach (FSMState state in fsmStates)
        {
            if (state.ID == currentStateID)
            {
                currentState = state;
                currentState.EnterState(playerTransform, transform);
                break;
            }
        }
    }
    #endregion
}
