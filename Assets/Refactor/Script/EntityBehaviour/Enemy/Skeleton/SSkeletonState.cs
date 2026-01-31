using UnityEngine.Assertions;

namespace EnemyBehaviour
{
    internal class SSkeletonState : SEnemyState
    {
        protected new CSkeletonStateMachine enemyStateMachine;
        public SSkeletonState(CEnemyStateMachine _stateMachine, MEnemyBehaviour _entity) : base(_stateMachine, _entity)
        {
            Assert.IsTrue(_stateMachine is CSkeletonStateMachine, "´Ë×´Ì¬ÊôÓÚCSkeletonStateMachine");
            enemyStateMachine = _stateMachine as CSkeletonStateMachine;
        }

        public override void Enter()
        {
            base.Enter();
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