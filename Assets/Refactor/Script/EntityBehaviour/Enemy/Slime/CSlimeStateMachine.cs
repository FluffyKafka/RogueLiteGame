using EntityBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EnemyBehaviour
{
    internal class CSlimeStateMachine : CEnemyStateMachine
    {
        protected MEnemyBehaviour slime;

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
            slime = entity as MEnemyBehaviour;

            idle = new SSlimeIdle(this, slime);
            move = new SSlimeMove(this, slime);
            battleIdle = new SSlimeBattleIdle(this, slime);
            battleMove = new SSlimeBattleMove(this, slime);
            attack = new SSlimeAttack(this, slime);
            stunned = new SSlimeStunned(this, slime);
            dead = new SSlimeDead(this, slime);
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
            //StateChangeDebugMessage(_newState);
            base.ChangeState(_newState);
        }
    }
}

