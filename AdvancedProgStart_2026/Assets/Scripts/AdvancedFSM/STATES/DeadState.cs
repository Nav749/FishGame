using UnityEngine;

public enum EnemyType
{
    Harpooner,
    Flyer,
    Fisher
}

public class DeadState : FSMState
{
    EnemyType enemyType;
    Harpooner h;
    Flyer f;

    Fisher fish;
    public DeadState(Harpooner harp)
    {
        stateID = StateID.Dead;
        enemyType = EnemyType.Harpooner;
        curSpeed = 0;
        curRotSpeed = 0;
        h = harp;
    }

    public DeadState(Flyer harp)
    {
        stateID = StateID.Dead;
        enemyType = EnemyType.Flyer;
        curSpeed = 0;
        curRotSpeed = 0;
        f = harp;
    }

    public DeadState(Fisher harp)
    {
        stateID = StateID.Dead;
        enemyType = EnemyType.Fisher;
        curSpeed = 0;
        curRotSpeed = 0;
        fish = harp;
    }

    public override void CheckTransitions(Transform player, Transform npc)
    {
        //i can't undie
    }

    public override void UpdateBehavior(Transform player, Transform npc)
    {
        if(enemyType == EnemyType.Harpooner)
        {
            h.death();
        }
        else if(enemyType == EnemyType.Flyer)
        {
            f.death();
        }
        else if(enemyType == EnemyType.Fisher)
        {
            fish.death();
        }
    }
}
