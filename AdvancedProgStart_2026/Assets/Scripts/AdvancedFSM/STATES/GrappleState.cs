using UnityEngine;

public class GrappleState : FSMState
{
    Fisher fisher;
    GrappleProp grappleProp;
    float time;
    Rigidbody rb;

    public GrappleState(Fisher f, GrappleProp g, Rigidbody r)
    {
        fisher = f;
        grappleProp = g;

        stateID = StateID.Grapple;

        curSpeed = grappleProp.speed;
        curRotSpeed = grappleProp.rotSpeed;
        time = grappleProp.waitTime;
        rb = r;
    }
    
    public override void CheckTransitions(Transform player, Transform npc)
    {
        if (fisher.Health == 0)
        {
            rb.linearVelocity = Vector3.zero;
            fisher.shooting = false;
            fisher.PerformTransition(TransitionID.NoHealth);
            return;
        }
        else if (fisher.hasShot)
        {
            rb.linearVelocity = Vector3.zero;
            fisher.shooting = false;
            fisher.StartCoroutine(fisher.Cooldown());
            fisher.PerformTransition(TransitionID.FarEnough);
            return;
        }
        else if (Vector3.Distance(player.position, npc.position) < 30f)
        {
            rb.linearVelocity = Vector3.zero;
            fisher.shooting = false;
            fisher.StartCoroutine(fisher.Cooldown());
            fisher.PerformTransition(TransitionID.FarEnough);
            return;
        }
    }

    public override void UpdateBehavior(Transform player, Transform npc)
    {
        fisher.LookatPlayer(player.position, curRotSpeed);

        if (!fisher.shooting)
        {
            fisher.StartCoroutine(fisher.FireBullet(time, true));
            //fisher.StartCoroutine(fisher.Cooldown());
            Debug.Log("dfhushfahdfosahfopiiahdfpoiahsfdpoihaspfdoiuhaspoiuihdsafopudhsafdjhaskfjhasofudnasfkvhsapi");
        }
    }
}
