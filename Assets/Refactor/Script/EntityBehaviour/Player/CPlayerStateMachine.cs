using EntityBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace PlayerBebaviour
{
    internal class CPlayerStateMachine : CEntityStateMachine
    {
        #region StateSet
        public SEntityState idle { get; protected set; }
        public SEntityState move { get; protected set; }
        public SEntityState jump { get; protected set; }
        public SEntityState fall { get; protected set; }
        public SEntityState wallSlide { get; protected set; }
        public SEntityState wallJump { get; protected set; }
        public SEntityState primaryAttack { get; protected set; }
        public SEntityState dead { get; protected set; }
        public SEntityState aim { get; protected set; }
        #endregion

        #region SkillStates
        public SEntityState dash { get; protected set; }
        #endregion

        #region InputHandle
        public float xInput { get; protected set; } = 0;
        public float yInput { get; protected set; } = 0;
        #endregion

        protected MPlayerBeviour player;

        protected bool isPlayerShouldBeIdle = false;

        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(entity is MPlayerBeviour, "Player状态机组件需要被附加至一个Player实体");
            player = entity as MPlayerBeviour;

            idle = new SPlayerIdle(this, entity);
            move = new SPlayerMove(this, entity);
            jump = new SPlayerJump(this, entity);
            fall = new SPlayerFall(this, entity);
            wallSlide = new SPlayerWallSlide(this, entity);
            wallJump = new SPlayerWallJump(this, entity);
            primaryAttack = new SPlayerPrimaryAttack(this, entity);
            dead = new SPlayerDead(this, entity);

            //SkillStats
            dash = new SPlayerDash(this, entity);
            aim = new SPlayerAimming(this, entity);

            player.CanEffectBehaviourSkillNotice += () => { return currentState is SPlayerRegularState; };
        }

        protected override void Start()
        {
            base.Start();
            Initialize(idle);
        }

        protected override void Update()
        {
            xInput = player.InvokeFunc(player.CheckHorizonInput);
            yInput = player.InvokeFunc(player.CheckVerticalInput);
            base.Update();
        }

        public override void ChangeState(SEntityState _newState)
        {
            if (isPlayerShouldBeIdle && currentState == idle)
            {
                return;
            }

            base.ChangeState(_newState);
        }

        public void BusyFor(float _duration)
        {
            StartCoroutine(BusyForHelper(_duration));
        }
        protected IEnumerator BusyForHelper(float _duration)
        {
            isPlayerShouldBeIdle = true;
            yield return new WaitForSeconds(_duration);
            isPlayerShouldBeIdle = false;
        }

        protected override void Die()
        {
            ChangeState(dead);
            isDenyStateChange = true;
        }
    }
}

