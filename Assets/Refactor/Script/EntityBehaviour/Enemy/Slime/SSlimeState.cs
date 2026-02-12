using EntityBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EnemyBehaviour
{
    internal class SSlimeState : SEnemyState
    {
        protected new CSlimeStateMachine enemyStateMachine;
        public SSlimeState(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
            Assert.IsTrue(_stateMachine is CSlimeStateMachine, "´Ë×´Ì¬ÊôÓÚCSlimeStateMachine");
            enemyStateMachine = _stateMachine as CSlimeStateMachine;
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
