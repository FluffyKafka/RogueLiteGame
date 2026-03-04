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

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            player.InvokeAction(player.StandStillNotice);
        }
    }
}

