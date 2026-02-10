
namespace EnemyBehaviour
{
    internal class SArcherPullBackJump : SArcherBattle
    {
        public SArcherPullBackJump(CArcherStateMachine _stateMachine, MEnemyBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            enemy.InvokeAction(enemy.ToPullbackJump);
            if (!enemy.InvokeFunc(enemy.TryEffectPullBackJump))
            {
                enemyStateMachine.ChangeState(enemyStateMachine.battleIdle);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();

            if(enemy.InvokeFunc(enemy.IsFall))
            {
                enemy.InvokeAction(enemy.ToFall);
            }

            if(enemy.InvokeFunc(enemy.IsGroundedOrPlatForm))
            {
                enemyStateMachine.ChangeState(enemyStateMachine.battleIdle);
            }
        }
    }
}
