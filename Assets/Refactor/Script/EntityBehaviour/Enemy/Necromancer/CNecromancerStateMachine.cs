using EntityBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EnemyBehaviour
{
    internal class CNecromancerStateMachine : CEntityStateMachine
    {
        protected MEnemyBehaviour enemy;

        #region StateSet
        public SEntityState idle { get; protected set; }
        public SEntityState move { get; protected set; }
        public SEntityState battleIdle { get; protected set; }
        public SEntityState battleMove { get; protected set; }
        public SEntityState attack { get; protected set; }
        public SEntityState dead { get; protected set; }
        public SEntityState controll { get; protected set; }
        #endregion
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(entity is MEnemyBehaviour, "AArcher状态机组件需要被附加至一个AArcher实体");
            enemy = entity as MEnemyBehaviour;

            idle = new SNecromancerIdle(this, enemy);
            move = new SNecromancerMove(this, enemy);
            battleIdle = new SNecromancerBattleIdle(this, enemy);
            battleMove = new SNecromancerBattleMove(this, enemy);
            attack = new SNecromancerAttack(this, enemy);
            dead = new SNecromancerDead(this, enemy);
            controll = new SNecromancerControll(this, enemy);
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

