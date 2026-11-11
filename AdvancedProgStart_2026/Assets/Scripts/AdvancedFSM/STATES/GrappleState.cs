using UnityEngine;

public class GrappleState : FSMState
{
    Fisher fisher;
    GrappleProp grappleProp;
    Transform fireLocation;
    Rigidbody rb;

    public GrappleState(Fisher f, GrappleProp g, Transform t, Rigidbody r)
    {
        fisher = f;
        grappleProp = g;

        stateID = StateID.Grapple;

        curSpeed = grappleProp.speed;
        curRotSpeed = grappleProp.rotSpeed;
        fireLocation = t;
        rb = r;
    }
    
    public override void CheckTransitions(Transform player, Transform npc)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateBehavior(Transform player, Transform npc)
    {
        throw new System.NotImplementedException();
    }
}
