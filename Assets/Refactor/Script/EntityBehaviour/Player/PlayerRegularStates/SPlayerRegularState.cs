using EntityBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBebaviour
{
    internal class SPlayerRegularState : SPlayerState
    {
        public SPlayerRegularState(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.DashBeginNotice += ToDash;
        }

        public override void Exit()
        {
            base.Exit();
            player.DashBeginNotice -= ToDash;
        }

        public override void Update()
        {
            base.Update();
        }

        protected void ToDash(float _speed)
        {
            playerStateMachine.ChangeState(playerStateMachine.dash);
        }
    }
}

