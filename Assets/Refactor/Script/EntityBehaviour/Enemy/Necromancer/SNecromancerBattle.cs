using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviour
{
    internal class SNecromancerBattle : SNecromancerState
    {
        public SNecromancerBattle(CNecromancerStateMachine _stateMachine, MEnemyBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            enemy.StopBattle += StopBattle;
            enemy.Attack += Attack;
            enemy.InvokeAction(enemy.ToBattle, true);
        }

        public override void Exit()
        {
            base.Exit();
            enemy.StopBattle -= StopBattle;
            enemy.Attack -= Attack;
            enemy.InvokeAction(enemy.ToBattle, false);
        }

        public override void Update()
        {
            base.Update();
            enemy.InvokeAction(enemy.UpdateBattle);
            enemy.InvokeAction(enemy.AttackCheck);
            enemy.InvokeAction(enemy.FacingToPlayer);
        }

        protected void StopBattle()
        {
            enemyStateMachine.ChangeState(enemyStateMachine.idle);
        }

        protected bool IdleCheckWithFlip()
        {
            int moveDir = enemy.InvokeFunc(enemy.CheckBattleMoveDir);
            if (moveDir == 0)
            {
                return true;
            }
            else
            {
                if (!enemy.InvokeFunc(enemy.IsGroundedOrPlatForm) || enemy.InvokeFunc(enemy.IsTouchWall))
                {
                    if (moveDir == enemy.InvokeFunc(enemy.CheckFacingDir))
                    {
                        return true;
                    }
                    else
                    {
                        enemy.InvokeAction(enemy.Flip);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        protected void Attack()
        {
            enemyStateMachine.ChangeState(enemyStateMachine.attack);
        }
    }
}

