using EntitySystem.EntityActor;
using EntitySystem.EntityComponent.StateMachineComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem
{
    namespace EntityState
    {
        namespace PlayerState
        {
            internal class SPlayerGround : SPlayerState
            {
                public SPlayerGround(CEntityStateMachine _stateMachine, AEntity _entity, string _animName) : base(_stateMachine, _entity, _animName)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                    player.JumpInput += Jump;
                }

                public override void Exit()
                {
                    base.Exit();
                    player.JumpInput -= Jump;
                }

                public override void Update()
                {
                    base.Update();
                }

                protected void Jump()
                {
                    playerStateMachine.ChangeState(playerStateMachine.jump);
                }
            }
        }
    }
}
