using UnityEngine.Assertions;

namespace EnemyBehaviour
{
    internal class SArcherState : SEnemyState
    {
        protected new CArcherStateMachine enemyStateMachine;
        public SArcherState(CArcherStateMachine _stateMachine, MEnemyBehaviour _entity) : base(_stateMachine, _entity)
        {
            enemyStateMachine = _stateMachine;
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
