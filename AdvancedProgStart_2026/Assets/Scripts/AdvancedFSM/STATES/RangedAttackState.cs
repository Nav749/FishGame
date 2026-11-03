using System.Collections;
using UnityEngine;

public class RangedAttackState : FSMState
{
    AttackProp attackProp;
    Harpooner harpooner;
    GameObject bulletPrefab;
    bool shooting;

    public RangedAttackState(Harpooner harp, AttackProp a)
    {
        attackProp = a;
        harpooner = harp;

        stateID = StateID.Attack;
        bulletPrefab = attackProp.bullet;
        curSpeed = attackProp.speed;
        curRotSpeed = attackProp.rotSpeed;

        shooting = false;
    }

    public override void CheckTransitions(Transform player, Transform npc)
    {
        throw new System.NotImplementedException();
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
