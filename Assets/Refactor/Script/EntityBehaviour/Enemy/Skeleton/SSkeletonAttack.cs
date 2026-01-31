
namespace EnemyBehaviour
{
    internal class SSkeletonAttack : SSkeletonState
    {
        public SSkeletonAttack(CEnemyStateMachine _stateMachine, MEnemyBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            enemy.AttackFinish += OnAttackFinish;
            enemy.BeStunned += OnStun;
        }

        public override void Exit()
        {
            base.Exit();
            enemy.AttackFinish -= OnAttackFinish;
            enemy.BeStunned -= OnStun;
        }

        public override void Update()
        {
            base.Update();
        }

        protected void OnAttackFinish()
        {
            enemyStateMachine.ChangeState(enemyStateMachine.battleIdle);
        }

        protected void OnStun()
        {
            enemyStateMachine.ChangeState(enemyStateMachine.stunned);
        }
    }
}
