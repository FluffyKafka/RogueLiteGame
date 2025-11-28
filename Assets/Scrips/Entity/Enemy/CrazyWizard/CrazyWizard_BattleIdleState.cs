using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyWizard_BattleIdleState : CrazyWizrd_StateBase
{
    private int moveDir;
    private Transform player;

    public CrazyWizard_BattleIdleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, CrazyWizrd _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer = enemy.battleDuration;
        player = PlayerManager.instance.player.transform;
        CalculateMoveDir();
    }

    public override void Update()
    {
        base.Update();
        CalculateMoveDir();
        if(TryAttack())
        {
            return;
        }

        if(TryIdle())
        {
            return;
        }

        if(TryBattleMove())
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

    private bool TryBattleMove()
    {
        if (((enemy.IsTouchWall() || !enemy.IsGrounded()) && moveDir == enemy.facingDir) || (enemy.IsDetectPlayerFront() && enemy.IsDetectPlayerFront().distance < enemy.toAttackRadius/2))
        {
            enemy.SetVelocityWithoutFlip(0, enemy.rg.velocity.y);
            return false;
        }
        else
        {
            enemy.stateMachine.ChangeState(enemy.battleMoveState);
            return true;
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
