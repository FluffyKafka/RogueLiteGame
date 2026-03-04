using EntityBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBebaviour
{
    internal class SPlayerCounterAttackSuccess : SPlayerSkillState
    {
        public SPlayerCounterAttackSuccess(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.ToCounterAttackSuccess();
            player.OnCounterAttackSuccessFinish += CounterAttackFinish;
        }

        public override void Exit()
        {
            base.Exit();
            player.OnCounterAttackSuccessFinish -= CounterAttackFinish;
        }

        public override void Update()
        {
            base.Update();
            player.InvokeAction(player.StandStillNotice);
        }
        protected void CounterAttackFinish()
        {
            playerStateMachine.ChangeState(playerStateMachine.idle);
        }
    }
}

