using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer_MoveState : NecromancerStateBase
{
    public Necromancer_MoveState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Necromancer _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
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

        if (enemy.IsDetectPlayerFront() || enemy.IsPlayerDetected())
        {
            stateMachine.ChangeState(enemy.battleState);
        }
        else if (enemy.IsTouchWall())
        {
            enemy.SetVelocity(enemy.moveSpeed * -enemy.facingDir, enemy.rg.velocity.y);
        }
        else if (!enemy.IsGrounded() && !enemy.IsPlatform())
        {
            stateMachine.ChangeState(enemy.idleState);
        }
        else
        {
            if (enemy.CheckGrounded().distance < enemy.flyingHeight)
            {
                enemy.SetVelocity(enemy.rg.velocity.x, enemy.verticalFlyingSpeed);
            }
            else if (enemy.CheckGrounded().distance > enemy.flyingHeight )
            {
                enemy.SetVelocity(enemy.rg.velocity.x, -enemy.verticalFlyingSpeed);
            }
            else
            {
                enemy.SetVelocity(enemy.rg.velocity.x, 0);
            }

            enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, enemy.rg.velocity.y);
        }
    }
}
