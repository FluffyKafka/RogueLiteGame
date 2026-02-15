using EntityBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBebaviour
{
    internal class SPlayerDash : SPlayerSkillState
    {
        public SPlayerDash(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.OnDashEnd += DashFinish;
        }

        public override void Exit()
        {
            base.Exit();
            player.OnDashEnd -= DashFinish;
        }

        public override void Update()
        {
            base.Update();
            player.InvokeAction(player.OnDashMovementUpdate);
        }

        protected void DashFinish()
        {
            playerStateMachine.ChangeState(playerStateMachine.idle);
        }
    }
}

