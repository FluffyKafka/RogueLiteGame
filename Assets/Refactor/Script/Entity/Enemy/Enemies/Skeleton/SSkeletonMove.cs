using EntitySystem.EntityActor;
using EntitySystem.EntityComponent.StateMachineComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem
{
    namespace EntityState
    {
        namespace SkeletonState
        {
            internal class SSkeletonMove : SSkeletonPeace
            {
                public SSkeletonMove(CEntityStateMachine _stateMachine, AEntity _entity) : base(_stateMachine, _entity)
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
    }
}
