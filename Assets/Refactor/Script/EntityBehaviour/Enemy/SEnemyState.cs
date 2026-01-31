using EntityBehaviour;
using UnityEngine.Assertions;

namespace EnemyBehaviour
{
    internal class SEnemyState : SEntityState
    {
        protected MEnemyBehaviour enemy;
        protected CEnemyStateMachine enemyStateMachine;
        public SEnemyState(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
            Assert.IsTrue(_entity is MEnemyBehaviour, "´Ë×´Ì¬ÊôÓÚMEnemyBehaviour");
            enemy = _entity as MEnemyBehaviour;
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