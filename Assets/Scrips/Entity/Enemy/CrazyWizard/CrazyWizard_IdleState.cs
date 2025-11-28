using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyWizard_IdleState : CrazyWizrd_StateBase
{
    public CrazyWizard_IdleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, CrazyWizrd _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer = Random.Range(enemy.minIdleDuration, enemy.maxIdleDuration);
        enemy.SetVelocityWithoutFlip(0, enemy.rg.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        SceneAudioManager.instance.skeletonSFX.roar.Play(enemy.transform);
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsDetectPlayerFront() || enemy.IsPlayerDetected())
        {
            float moveDir = 1;
            if (PlayerManager.instance.player.transform.position.x < enemy.transform.position.x)
            {
                moveDir = -1;
            }
            if ((enemy.IsTouchWall() || !enemy.IsGrounded()) && moveDir == enemy.facingDir)
            {
                timer = Random.Range(enemy.minIdleDuration, enemy.maxIdleDuration);
            }
            else
            {
                stateMachine.ChangeState(enemy.battleIdleState);
            }

        }
        if (timer < 0)
        {
            stateMachine.ChangeState(enemy.wanderState);
        }
    }
}
