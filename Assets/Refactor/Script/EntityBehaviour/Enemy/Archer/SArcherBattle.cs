
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
        }

        protected void StopBattle()
        {
            enemyStateMachine.ChangeState(enemyStateMachine.idle);
        }

        protected bool TryBattleStateChange()
        {
            SArcherBattle targetBattleState;
            if(enemy.InvokeFunc(enemy.CheckBattleMoveDir) != 0)
            {
                targetBattleState = enemyStateMachine.battleMove as SArcherBattle;
            }
            else if(enemy.InvokeFunc(enemy.CanPullBackJump))
            {
                targetBattleState = enemyStateMachine.pullBackJump as SArcherBattle;
            }
            else if(enemy.InvokeFunc(enemy.CanPullBack))
            {
                targetBattleState = enemyStateMachine.pullBack as SArcherBattle;
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
