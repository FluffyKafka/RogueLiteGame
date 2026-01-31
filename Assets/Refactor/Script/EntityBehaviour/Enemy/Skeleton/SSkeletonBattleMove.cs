
namespace EnemyBehaviour
{
    internal class SSkeletonBattleMove : SSkeletonBattle
    {
        public SSkeletonBattleMove(CEnemyStateMachine _stateMachine, MEnemyBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            enemy.InvokeAction(enemy.ToMove);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            if (IdleCheckWithFlip())
            {
                enemyStateMachine.ChangeState(enemyStateMachine.battleIdle);
            }
            else
            {
                enemy.InvokeAction(enemy.MoveToward_Battle, enemy.InvokeFunc(enemy.CheckBattleMoveDir));
            }
        }
    }
}