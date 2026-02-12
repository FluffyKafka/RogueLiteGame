using EntityBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviour
{
    internal class SSlimeMove : SSlimePeace
    {
        public SSlimeMove(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
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

