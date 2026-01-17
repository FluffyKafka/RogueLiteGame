using EntitySystem.EntityActor.EnemyActor;
using EntitySystem.EntityActor.PlayerActor;
using EntitySystem.EntityState;
using EntitySystem.EntityState.PlayerState;
using EntitySystem.EntityState.SkeletonState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EntitySystem
{
    namespace EntityComponent
    {
        namespace StateMachineComponent
        {
            internal class CSkeletonStateMachine : CEntityStateMachine
            {
                protected ASkeleton skeleton;

                #region StateSet
                public SEntityState idle { get; protected set; }
                public SEntityState move { get; protected set; }
                public SEntityState battleIdle { get; protected set; }
                public SEntityState battleMove { get; protected set; }
                public SEntityState attack { get; protected set; }
                public SEntityState stunned { get; protected set; }
                public SEntityState dead { get; protected set; }
                #endregion
                protected override void Awake()
                {
                    base.Awake();

                    Assert.IsTrue(entity is ASkeleton, "ASkeleton状态机组件需要被附加至一个ASkeleton实体");
                    skeleton = entity as ASkeleton;

                    idle = new SSkeletonIdle(this, entity);
                    move = new SSkeletonMove(this, entity);
                    battleIdle = new SSkeletonBattleIdle(this, entity);
                    battleMove = new SSkeletonBattleMove(this, entity);
                    attack = new SSkeletonAttack(this, entity);
                    stunned = new SSkeletonStunned(this, entity);
                    dead = new SSkeletonDead(this, entity);
                }

                protected override void Start()
                {
                    base.Start();
                    Initialize(idle);
                }

                protected override void Die()
                {
                    ChangeState(dead);
                    isDenyStateChange = true;    
                }

                public override void ChangeState(SEntityState _newState)
                {
                    base.ChangeState(_newState);
                }
            }
        }
    }
}