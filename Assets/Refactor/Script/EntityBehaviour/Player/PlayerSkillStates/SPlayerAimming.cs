using EntityBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBebaviour
{
    internal class SPlayerAimming : SPlayerSkillState
    {
        public SPlayerAimming(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.OnAimmingFinish += AimmingFinish;
        }

        public override void Exit()
        {
            base.Exit();
            player.OnAimmingFinish -= AimmingFinish;
        }

        public override void Update()
        {
            base.Update();
            player.InvokeAction(player.StandStillNotice);            
        }

        protected void AimmingFinish()
        {
            playerStateMachine.ChangeState(playerStateMachine.idle);
        }
    }
}
