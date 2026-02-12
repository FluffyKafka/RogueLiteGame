using EntityBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviour
{
    internal class SSlimePeace : SSlimeState
    {
        public SSlimePeace(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
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
            if (enemy.InvokeFunc(enemy.IsDetectPlayer))
            {
                stateMachine.ChangeState(enemyStateMachine.battleIdle);
            }
        }
    }
}

