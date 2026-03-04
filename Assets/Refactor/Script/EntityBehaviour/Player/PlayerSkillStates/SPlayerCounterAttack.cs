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
            player.ToCounterAttack();
            player.OnCounterAttackEnd += OnCounterEnd;
        }

        public override void Exit()
        {
            base.Exit();
            player.OnCounterAttackEnd -= OnCounterEnd;
        }

        public override void Update()
        {
            base.Update();
            player.InvokeAction(player.StandStillNotice);
            if(player.InvokeFunc(player.CounterAttackCheckNotice))
            {
                playerStateMachine.ChangeState(playerStateMachine.counterAttackSuccess);
            }
        }

        protected void OnCounterEnd()
        {
            playerStateMachine.ChangeState(playerStateMachine.idle);
        }
    }
}

