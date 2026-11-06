using UnityEngine;

public class DeadState : FSMState
{
    public DeadState()
    {
        stateID = StateID.Dead;

        curSpeed = 0;
        curRotSpeed = 0;
    }

    public override void CheckTransitions(Transform player, Transform npc)
    {
        //i can't undie
    }

    public override void UpdateBehavior(Transform player, Transform npc)
    {
        //I'm Dead, no need to do shit
    }
}
