using System.Collections;
using UnityEngine;

public class RangedAttackState : FSMState
{
    AttackProp attackProp;
    Harpooner harpooner;
    bool shooting;

    public RangedAttackState(Harpooner harp, AttackProp a)
    {
        attackProp = a;
        harpooner = harp;

        stateID = StateID.Attack;
        curSpeed = attackProp.speed;
        curRotSpeed = attackProp.rotSpeed;

        shooting = false;
    }

    public override void CheckTransitions(Transform player, Transform npc)
    {
        if (harpooner.Health == 0)
        {
            shooting = false;
            harpooner.PerformTransition(TransitionID.NoHealth);
            return;
        }
        else if(Vector3.Distance(player.position, npc.position) < 30f)
        {
            shooting = false;
            harpooner.PerformTransition(TransitionID.TooClose);
            return;
        }
    }

    public override void UpdateBehavior(Transform player, Transform npc)
    {
        while (!shooting)
        {
            shooting = true;
            harpooner.StartCoroutine(harpooner.FireBullet());
            shooting = false;
        }
    }
}
