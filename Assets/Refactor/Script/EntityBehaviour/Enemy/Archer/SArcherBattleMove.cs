
namespace EnemyBehaviour
{
    internal class SArcherBattleMove : SArcherBattle
    {
        public SArcherBattleMove(CArcherStateMachine _stateMachine, MEnemyBehaviour _entity) : base(_stateMachine, _entity)
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
            if (!TryBattleStateChange())
            {
                enemy.InvokeAction(enemy.MoveToward_Battle, enemy.InvokeFunc(enemy.CheckBattleMoveDir));
            }
        }
    }
}
