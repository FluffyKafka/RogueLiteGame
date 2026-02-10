
namespace EnemyBehaviour
{
    internal class SArcherAttack : SArcherState
    {
        public SArcherAttack(CArcherStateMachine _stateMachine, MEnemyBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            enemy.AttackFinish += OnAttackFinish;
        }

        public override void Exit()
        {
            base.Exit();
            enemy.AttackFinish -= OnAttackFinish;
        }

        public override void Update()
        {
            base.Update();
        }

        protected void OnAttackFinish()
        {
            enemyStateMachine.ChangeState(enemyStateMachine.battleIdle);
        }
    }
}
