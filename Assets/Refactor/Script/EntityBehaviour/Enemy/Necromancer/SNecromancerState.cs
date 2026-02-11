using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviour
{
    internal class SNecromancerState : SEnemyState
    {
        protected new CNecromancerStateMachine enemyStateMachine;
        public SNecromancerState(CNecromancerStateMachine _stateMachine, MEnemyBehaviour _entity) : base(_stateMachine, _entity)
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
