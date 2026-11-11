using UnityEngine;

public class GrappleState : FSMState
{
    Fisher fisher;
    GrappleProp grappleProp;
    float time;

    public GrappleState(Fisher f, GrappleProp g)
    {
        fisher = f;
        grappleProp = g;

        stateID = StateID.Grapple;

        curSpeed = grappleProp.speed;
        curRotSpeed = grappleProp.rotSpeed;
        time = grappleProp.waitTime;
    }
    
    public override void CheckTransitions(Transform player, Transform npc)
    {
        if (fisher.Health == 0)
        {
            fisher.PerformTransition(TransitionID.NoHealth);
            return;
        }
        else if (Vector3.Distance(player.position, npc.position) < 30f && !fisher.shooting)
        {
            fisher.PerformTransition(TransitionID.FarEnough);
            return;
        }
        else if (Vector3.Distance(player.position, npc.position) < 5f && !fisher.shooting)
        {
            fisher.PerformTransition(TransitionID.TooClose);
            return;
        }
    }

    public override void UpdateBehavior(Transform player, Transform npc)
    {
        if (!fisher.shooting)
        {
            fisher.StartCoroutine(fisher.FireBullet(time, true));
            fisher.StartCoroutine(fisher.Cooldown());
        }
    }
}
