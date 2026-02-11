using EntityBehaviour;
using UnityEngine;
using UnityEngine.Assertions;

namespace EnemyBehaviour
{
    internal class CArcherStateMachine : CEnemyStateMachine
    {
        protected MEnemyBehaviour archer;

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

            Assert.IsTrue(entity is MEnemyBehaviour, "AArcher状态机组件需要被附加至一个AArcher实体");
            archer = entity as MEnemyBehaviour;

            idle = new SArcherIdle(this, archer);
            move = new SArcherMove(this, archer);
            battleIdle = new SArcherBattleIdle(this, archer);
            battleMove = new SArcherBattleMove(this, archer);
            pullBack = new SArcherPullBack(this, archer);
            pullBackJump = new SArcherPullBackJump(this, archer);
            attack = new SArcherAttack(this, archer);
            dead = new SArcherDead(this, archer);
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
