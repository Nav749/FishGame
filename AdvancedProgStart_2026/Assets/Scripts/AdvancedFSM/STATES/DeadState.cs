using UnityEngine;

public class DeadState : FSMState
{
    Harpooner h;
    public DeadState(Harpooner harp)
    {
        stateID = StateID.Dead;

        curSpeed = 0;
        curRotSpeed = 0;
        h = harp;
    }

    public override void CheckTransitions(Transform player, Transform npc)
    {
        //i can't undie
    }

    public override void UpdateBehavior(Transform player, Transform npc)
    {
        h.death();
    }
}
