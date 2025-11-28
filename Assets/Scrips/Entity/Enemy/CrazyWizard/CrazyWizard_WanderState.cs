using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyWizard_WanderState : CrazyWizrd_StateBase
{
    public CrazyWizard_WanderState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, CrazyWizrd _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (!enemy.IsGrounded() && !enemy.IsPlatform())
        {
            enemy.SetVelocity(enemy.moveSpeed * -enemy.facingDir, enemy.rg.velocity.y);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsTouchWall())
        {
            enemy.SetVelocity(enemy.moveSpeed * -enemy.facingDir, enemy.rg.velocity.y);
        }
        else if (!enemy.IsGrounded() && !enemy.IsPlatform())
        {
            stateMachine.ChangeState(enemy.idleState);
        }
        else
        {
            enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, enemy.rg.velocity.y);
        }

        if (enemy.IsDetectPlayerFront() || enemy.IsPlayerDetected())
        {
            stateMachine.ChangeState(enemy.battleIdleState);
        }
    }
}
