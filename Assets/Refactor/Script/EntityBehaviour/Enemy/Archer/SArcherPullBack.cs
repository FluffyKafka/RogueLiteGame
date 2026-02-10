
namespace EnemyBehaviour
{
    internal class SArcherPullBack : SArcherBattle
    {
        public SArcherPullBack(CArcherStateMachine _stateMachine, MEnemyBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            enemy.InvokeAction(enemy.ToPullBack);
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
                enemy.InvokeAction(enemy.PullBackUpdate);
            }
        }
    }
}
