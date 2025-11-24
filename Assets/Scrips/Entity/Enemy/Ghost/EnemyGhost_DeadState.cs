using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhost_DeadState : EnemyGhost_StateBase
{
    public EnemyGhost_DeadState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName,
        EnemyGhost _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetVelocity(0, 0);

        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.cd.enabled = false;


        timer = enemy.deadAnimDuration;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (timer > 0)
        {
            enemy.rg.velocity = enemy.deadAnimVelocity;
        }
    }
}
