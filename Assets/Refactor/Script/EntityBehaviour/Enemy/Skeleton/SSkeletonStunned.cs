
namespace EnemyBehaviour
{
    internal class SSkeletonStunned : SSkeletonState
    {
        public SSkeletonStunned(CEnemyStateMachine _stateMachine, MEnemyBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            enemy.StunFinish += StunFinish;
        }

        public override void Exit()
        {
            base.Exit();
            enemy.StunFinish -= StunFinish;
        }

        public override void Update()
        {
            base.Update();
        }

        protected void StunFinish()
        {
            enemyStateMachine.ChangeState(enemyStateMachine.battleIdle);
        }
    }
}