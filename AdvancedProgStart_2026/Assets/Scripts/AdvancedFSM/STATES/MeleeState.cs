using UnityEngine;

public class MeleeState : FSMState
{
    Fisher fisher;
    MeleeProp meleeProp;
    float waitTime;

    public MeleeState(Fisher fish, MeleeProp m)
    {
        fisher = fish;
        meleeProp = m;
        stateID = StateID.Melee;
        curSpeed = meleeProp.speed;
        curRotSpeed = meleeProp.rotSpeed;
        waitTime = meleeProp.waitTime;
    }

    public override void CheckTransitions(Transform player, Transform npc)
    {
        if(fisher.Health == 0)
        {
            fisher.PerformTransition(TransitionID.NoHealth);
            return;
        }
    }

    public override void UpdateBehavior(Transform player, Transform npc)
    {
        fisher.LookatPlayer(player.position, curRotSpeed);

        if (!fisher.shooting)
        {
            fisher.StartCoroutine(fisher.FireBullet(waitTime));
        }
    }
}
