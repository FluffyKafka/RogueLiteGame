
namespace EnemyBehaviour
{
    internal class SSkeletonBattleIdle : SSkeletonBattle
    {
        public SSkeletonBattleIdle(CEnemyStateMachine _stateMachine, MEnemyBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            enemy.InvokeAction(enemy.StandStill);
            enemy.InvokeAction(enemy.ToIdle);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            if (!IdleCheckWithFlip())
            {
                enemyStateMachine.ChangeState(enemyStateMachine.battleMove);
            }
        }
    }
}
