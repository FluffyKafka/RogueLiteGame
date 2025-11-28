using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyWizard_BattleMoveState : CrazyWizrd_StateBase
{
    private int moveDir;
    private Transform player;

    public CrazyWizard_BattleMoveState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, CrazyWizrd _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer = enemy.battleDuration;
        player = PlayerManager.instance.player.transform;
        CalculateMoveDir();
        TryBattleIdle();
    }

    public override void Update()
    {
        base.Update();
        CalculateMoveDir();
        if (TryAttack())
        {
            return;
        }

        if (TryIdle())
        {
            return;
        }

        if (TryBattleIdle())
        {
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void CalculateMoveDir()
    {
        if (player.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }
        else if (player.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }
    }

    private bool TryBattleIdle()
    {
        if (((enemy.IsTouchWall() || !enemy.IsGrounded()) && moveDir == enemy.facingDir) || (enemy.IsDetectPlayerFront() && enemy.IsDetectPlayerFront().distance < enemy.toAttackRadius / 2))
        {
            enemy.stateMachine.ChangeState(enemy.battleIdleState);
            return true;
        }
        else
        {
            enemy.SetVelocity(enemy.battleMoveSpeed * moveDir, enemy.rg.velocity.y);
            return false;
        }
    }

    private bool TryAttack()
    {
        if (enemy.IsDetectPlayerFront())
        {
            timer = enemy.battleDuration;
            if (enemy.IsDetectPlayerFront().distance < enemy.toAttackRadius)
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                    return true;
                }
            }
        }
        return false;
    }
    private bool TryIdle()
    {
        if (!enemy.IsDetectPlayerFront())
        {
            if (timer < 0)
            {
                stateMachine.ChangeState(enemy.idleState);
                return true;
            }
        }
        return false;
    }
    private bool CanAttack()
    {
        return (Time.time - enemy.lastAttackTime) > Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
    }
}
