
namespace EnemyBehaviour
{
    internal class SSkeletonDead : SSkeletonState
    {
        public SSkeletonDead(CEnemyStateMachine _stateMachine, MEnemyBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            enemy.InvokeAction(enemy.ToDead);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
