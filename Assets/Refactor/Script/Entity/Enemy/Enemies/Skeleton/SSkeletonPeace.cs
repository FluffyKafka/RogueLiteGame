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
            internal class SSkeletonPeace : SSkeletonState
            {
                public SSkeletonPeace(CEntityStateMachine _stateMachine, AEntity _entity, string _animName) : base(_stateMachine, _entity, _animName)
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
    }
}