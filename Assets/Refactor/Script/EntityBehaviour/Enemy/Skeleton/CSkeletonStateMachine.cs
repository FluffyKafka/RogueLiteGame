using EntityBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EnemyBehaviour
{
    internal class CSkeletonStateMachine : CEnemyStateMachine
    {
        protected MEnemyBehaviour skeleton;

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

            Assert.IsTrue(entity is MEnemyBehaviour, "ASkeleton状态机组件需要被附加至一个ASkeleton实体");
            skeleton = entity as MEnemyBehaviour;

            idle = new SSkeletonIdle(this, skeleton);
            move = new SSkeletonMove(this, skeleton);
            battleIdle = new SSkeletonBattleIdle(this, skeleton);
            battleMove = new SSkeletonBattleMove(this, skeleton);
            attack = new SSkeletonAttack(this, skeleton);
            stunned = new SSkeletonStunned(this, skeleton);
            dead = new SSkeletonDead(this, skeleton);
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