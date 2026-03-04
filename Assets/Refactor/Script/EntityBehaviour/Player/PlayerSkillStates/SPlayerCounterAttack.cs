using EntityBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBebaviour
{
    internal class SPlayerCounterAttack : SPlayerSkillState
    {
        public SPlayerCounterAttack(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        protected bool isSuccess = false;

        public override void Enter()
        {
            base.Enter();
            player.ToCounterAttack();
            player.OnCounterAttackEnd += OnCounterEnd;
            isSuccess = false;
        }

        public override void Exit()
        {
            base.Exit();
            player.OnCounterAttackEnd -= OnCounterEnd;
            if(isSuccess)
            {
                player.OnCounterAttackSuccessFinish -= OnCounterEnd;
            }
        }

        public override void Update()
        {
            base.Update();
            player.InvokeAction(player.StandStillNotice);
            TryCounterAttack();
        }

        protected void TryCounterAttack()
        {
            if(isSuccess)
            {
                return;
            }

            isSuccess = player.InvokeFunc(player.CounterAttackCheckNotice);
            if (isSuccess)
            {
                player.ToCounterAttackSuccess();
                player.OnCounterAttackSuccessFinish += OnCounterEnd;
            }
        }

        protected void OnCounterEnd()
        {
            playerStateMachine.ChangeState(playerStateMachine.idle);
        }
    }
}

