

namespace EnemyBehaviour
{
    internal class SArcherBattleIdle : SArcherBattle
    {
        public SArcherBattleIdle(CArcherStateMachine _stateMachine, MEnemyBehaviour _entity) : base(_stateMachine, _entity)
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
            TryBattleStateChange();
        }
    }
}
