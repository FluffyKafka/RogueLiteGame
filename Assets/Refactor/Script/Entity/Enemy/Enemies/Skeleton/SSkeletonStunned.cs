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
            internal class SSkeletonStunned : SSkeletonState
            {
                public SSkeletonStunned(CEntityStateMachine _stateMachine, AEntity _entity) : base(_stateMachine, _entity)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                    enemy.StunFinish += StunFinish;
                }

                public override void Exit()
                {
                    base.Exit();
                    enemy.StunFinish -= StunFinish;
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
    }
}