
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
            enemy.InvokeAction(enemy.ToBattle, true);
        }

        public override void Exit()
        {
            base.Exit();
            enemy.AttackFinish -= OnAttackFinish;
            enemy.InvokeAction(enemy.ToBattle, false);
        }

        public override void Update()
        {
            base.Update();
            enemy.InvokeAction(enemy.StandStill);
        }

        protected void OnAttackFinish()
        {
            enemyStateMachine.ChangeState(enemyStateMachine.battleIdle);
        }
    }
}
