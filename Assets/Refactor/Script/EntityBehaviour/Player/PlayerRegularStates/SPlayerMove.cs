using EntityBehaviour;
using System;
using UnityEngine;

namespace PlayerBebaviour
{
    internal class SPlayerMove : SPlayerGround
    {
        public SPlayerMove(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.InvokeAction(player.ToMove);
        }

        public override void Exit()
        {
            base.Exit();
            player.InvokeAction(player.ToExitMove);
        }

        public override void Update()
        {
            base.Update();
            player.InvokeAction(player.Move, playerStateMachine.xInput);
            if (MathF.Abs(playerStateMachine.xInput) < 0.0001)
            {
                playerStateMachine.ChangeState(playerStateMachine.idle);
            }
        }
    }
}
