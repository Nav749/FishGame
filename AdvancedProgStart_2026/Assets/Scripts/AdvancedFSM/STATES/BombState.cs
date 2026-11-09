using UnityEngine;
using UnityEngine.AI;

public class BombState : FSMState
{
    Flyer flyer;
    BombProp bombProp;

    public BombState(Flyer f, BombProp b)
    {
        flyer = f;
        bombProp = b;
        stateID = StateID.Bomb;
        curSpeed = bombProp.speed;
        curRotSpeed = bombProp.rotSpeed;
    }

    public override void CheckTransitions(Transform player, Transform npc)
    {
        if(flyer.Health == 0)
        {
            flyer.PerformTransition(TransitionID.NoHealth);
            return;
        }
        else if(Vector3.Distance(player.position, new Vector3(npc.position.x, player.position.y, npc.position.z)) > 20f)
        {
            flyer.PerformTransition(TransitionID.FarEnough);
            return;
        }
        else if (flyer.hasShot)
        {
            flyer.PerformTransition(TransitionID.FarEnough);
            return;
        }
    }

    public override void UpdateBehavior(Transform player, Transform npc)
    {
        if (!flyer.shooting)
        {
            flyer.StartCoroutine(flyer.FireBullet());
            flyer.StartCoroutine(flyer.Cooldown());
        }
    }
}
