
namespace EnemyBehaviour
{
    internal class SArcherMove : SArcherPeace
    {
        public SArcherMove(CArcherStateMachine _stateMachine, MEnemyBehaviour _entity) : base(_stateMachine, _entity)
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

            if (enemy.InvokeFunc(enemy.IsTouchWall))
            {
                enemy.InvokeAction(enemy.MoveForward, -1);
            }
            else if (!enemy.InvokeFunc(enemy.IsGroundedOrPlatForm))
            {
                enemyStateMachine.ChangeState(enemyStateMachine.idle);
            }
            else
            {
                enemy.InvokeAction(enemy.MoveForward, 1);
            }
        }
    }
}
