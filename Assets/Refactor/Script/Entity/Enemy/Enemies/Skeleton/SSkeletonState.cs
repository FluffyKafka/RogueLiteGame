using EntitySystem.EntityActor;
using EntitySystem.EntityActor.EnemyActor;
using EntitySystem.EntityComponent.StateMachineComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EntitySystem
{
    namespace EntityState
    {
        namespace SkeletonState
        {
            internal class SSkeletonState : SEntityState
            {
                protected ASkeleton enemy;
                protected CSkeletonStateMachine enemyStateMachine;
                public SSkeletonState(CEntityStateMachine _stateMachine, AEntity _entity, string _animName) : base(_stateMachine, _entity, _animName)
                {
                    Assert.IsTrue(_entity is ASkeleton, "此状态属于Skeleton");
                    enemy = _entity as ASkeleton;

                    Assert.IsTrue(_stateMachine is CSkeletonStateMachine, "此状态属于Skeleton");
                    enemyStateMachine = _stateMachine as CSkeletonStateMachine;
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
    }
}

