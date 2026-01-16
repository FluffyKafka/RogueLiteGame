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
                [SerializeField] protected string idleAnimName = "Idle";
                public SEntityState move { get; protected set; }
                [SerializeField] protected string moveAnimName = "Move";
                public SEntityState battleIdle { get; protected set; }
                [SerializeField] protected string battleIdleName = "Idle";
                public SEntityState battleMove { get; protected set; }
                [SerializeField] protected string battleMoveName = "Move";
                public SEntityState attack { get; protected set; }
                [SerializeField] protected string attackAnimName = "Attack";
                public SEntityState stunned { get; protected set; }
                [SerializeField] protected string stunnedAnimName = "Stun";
                public SEntityState dead { get; protected set; }
                [SerializeField] protected string deadAnimName = "Idle";
                #endregion
                protected override void Awake()
                {
                    base.Awake();

                    Assert.IsTrue(entity is ASkeleton, "ASkeleton状态机组件需要被附加至一个ASkeleton实体");
                    skeleton = entity as ASkeleton;

                    idle = new SSkeletonIdle(this, entity, idleAnimName);
                    move = new SSkeletonMove(this, entity, moveAnimName);
                    battleIdle = new SSkeletonBattleIdle(this, entity, battleIdleName);
                    battleMove = new SSkeletonBattleMove(this, entity, battleMoveName);
                    attack = new SSkeletonAttack(this, entity, attackAnimName);
                    stunned = new SSkeletonStunned(this, entity, stunnedAnimName);
                    dead = new SSkeletonDead(this, entity, deadAnimName);
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
                    Debug.Log("from: " + currentState.GetType().Name + " to: " + _newState.GetType().Name);
                    base.ChangeState(_newState);
                }
            }
        }
    }
}

