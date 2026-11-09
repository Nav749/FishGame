using UnityEngine;

public class BombState : FSMState
{
    Flyer flyer;
    BombProp bombProp;

    public BombState(Flyer f, BombProp b)
    {
        flyer = f;
        bombProp = b;

        curSpeed = bombProp.speed;
        curRotSpeed = bombProp.rotSpeed;
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
