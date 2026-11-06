using UnityEngine;

public class RangedAttackState : FSMState
{
    AttackProp attackProp;
    Harpooner harpooner;
    GameObject bullet;
    bool shooting;

    public RangedAttackState(Harpooner harp, AttackProp a)
    {
        attackProp = a;
        harpooner = harp;

        stateID = StateID.Attack;
        bullet = attackProp.bullet;
        curSpeed = attackProp.speed;
        curRotSpeed = attackProp.rotSpeed;
    }

    public override void CheckTransitions(Transform player, Transform npc)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateBehavior(Transform player, Transform npc)
    {
        while (!shooting)
        {

        }
    }
}
