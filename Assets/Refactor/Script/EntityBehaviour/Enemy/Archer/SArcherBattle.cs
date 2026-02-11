


using UnityEngine;

namespace EnemyBehaviour
{
    internal class SArcherBattle : SArcherState
    {
        public SArcherBattle(CArcherStateMachine _stateMachine, MEnemyBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            enemy.StopBattle += StopBattle;
            enemy.Attack += Attack;
        }

        public override void Exit()
        {
            base.Exit();
            enemy.StopBattle -= StopBattle;
            enemy.Attack -= Attack;
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

        protected bool TryBattleStateChange()
        {
            SArcherBattle targetBattleState;

            int moveDir = enemy.InvokeFunc(enemy.CheckBattleMoveDir);
            int faceDir = enemy.InvokeFunc(enemy.CheckFacingDir);
            bool isGround = enemy.InvokeFunc(enemy.IsGroundedOrPlatForm);
            bool isWall = enemy.InvokeFunc(enemy.IsTouchWall);
            bool CanMove = isGround && !isWall;

            if (moveDir != 0)
            {
                if(moveDir == faceDir && !CanMove)
                {
                    targetBattleState = enemyStateMachine.battleIdle as SArcherBattle;
                }
                else
                {
                    targetBattleState = enemyStateMachine.battleMove as SArcherBattle;
                }
            }
            else if(enemy.InvokeFunc(enemy.CanPullBackJump))
            {                
                targetBattleState = enemyStateMachine.pullBackJump as SArcherBattle;
            }
            else if(enemy.InvokeFunc(enemy.CanPullBack))
            {
                if (!CanMove)
                {
                    targetBattleState = enemyStateMachine.battleIdle as SArcherBattle;
                }
                else
                {
                    targetBattleState = enemyStateMachine.pullBack as SArcherBattle;
                }
            }
            else
            {
                targetBattleState = enemyStateMachine.battleIdle as SArcherBattle;
            }

            if(this == targetBattleState)
            {
                return false;
            }
            else
            {
                enemyStateMachine.ChangeState(targetBattleState);
                return true;
            }
        }

        protected void Attack()
        {
            enemyStateMachine.ChangeState(enemyStateMachine.attack);
        }
    }
}
