using EntityBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviour
{
    internal class SSlimeStunned : SSlimeState
    {
        public SSlimeStunned(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            enemy.StunFinishNotice += StunFinish;
        }

        public override void Exit()
        {
            base.Exit();
            enemy.StunFinishNotice -= StunFinish;
        }

        public override void Update()
        {
            base.Update();
        }

        protected void StunFinish()
        {
            enemyStateMachine.ChangeState(enemyStateMachine.battleIdle);
        }
    }
}

