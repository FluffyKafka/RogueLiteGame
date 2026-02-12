using EntityBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EnemyBehaviour
{
    internal class CBombManStateMachine : CEnemyStateMachine
    {
        protected MEnemyBehaviour bombMan;

        #region StateSet
        public SEntityState idle { get; protected set; }
        public SEntityState move { get; protected set; }
        public SEntityState battleIdle { get; protected set; }
        public SEntityState battleMove { get; protected set; }
        public SEntityState attack { get; protected set; }
        public SEntityState stunned { get; protected set; }
        public SEntityState explodeHolding { get; protected set; }
        public SEntityState explode { get; protected set; }
        public SEntityState stunnedExplode { get; protected set; }
        public SEntityState dead { get; protected set; }
        #endregion
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(entity is MEnemyBehaviour, "ASkeleton状态机组件需要被附加至一个ASkeleton实体");
            bombMan = entity as MEnemyBehaviour;

            idle = new SBombManIdle(this, bombMan);
            move = new SBombManMove(this, bombMan);
            battleIdle = new SBombManBattleIdle(this, bombMan);
            battleMove = new SBombManBattleMove(this, bombMan);
            attack = new SBombManAttack(this, bombMan);
            stunned = new SBombManStunned(this, bombMan);
            explodeHolding = new SBombManExplodeHolding(this, bombMan);
            explode = new SBombManExplode(this, bombMan);
            stunnedExplode = new SBombManStunnedExlpode(this, bombMan);
            dead = new SBombManDead(this, bombMan);
        }

        protected override void Start()
        {
            base.Start();
            Initialize(idle);
        }

        protected override void Die()
        {
            ChangeState(explodeHolding);
        }

        public override void ChangeState(SEntityState _newState)
        {
            base.ChangeState(_newState);
        }
    }
}