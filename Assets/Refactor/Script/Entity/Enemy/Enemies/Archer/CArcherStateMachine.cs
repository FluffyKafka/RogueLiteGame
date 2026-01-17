using EntitySystem.EntityActor.EnemyActor;
using EntitySystem.EntityState;
using EntitySystem.EntityState.ArcherState;
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
            internal class CArcherStateMachine : CEntityStateMachine
            {
                protected AArcher archer;

                #region StateSet
                public SEntityState idle { get; protected set; }
                public SEntityState move { get; protected set; }
                public SEntityState battleIdle { get; protected set; }
                public SEntityState battleMove { get; protected set; }
                public SEntityState pullBack { get; protected set; }
                public SEntityState pullBackJump { get; protected set; }
                public SEntityState attack { get; protected set; }
                public SEntityState dead { get; protected set; }
                #endregion
                protected override void Awake()
                {
                    base.Awake();

                    Assert.IsTrue(entity is AArcher, "AArcher状态机组件需要被附加至一个AArcher实体");
                    archer = entity as AArcher;

                    idle = new SArcherIdle(this, entity);
                    move = new SArcherMove(this, entity);
                    battleIdle = new SArcherBattleIdle(this, entity);
                    battleMove = new SArcherBattleMove(this, entity);
                    pullBack = new SArcherPullBack(this, entity);
                    pullBackJump = new SArcherPullBack(this, entity);
                    attack = new SArcherAttack(this, entity);
                    dead = new SArcherDead(this, entity);
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
