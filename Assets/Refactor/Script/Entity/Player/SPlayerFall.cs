using EntitySystem.EntityActor;
using EntitySystem.EntityComponent.StateMachineComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Windows;

namespace EntitySystem
{
    namespace EntityState
    {
        namespace PlayerState
        {
            internal class SPlayerFall : SPlayerAir
            {
                public SPlayerFall(CEntityStateMachine _stateMachine, AEntity _entity) : base(_stateMachine, _entity)
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

                    if (player.InvokeFunc(player.IsGroundedOrPlatForm))
                    {
                        playerStateMachine.ChangeState(playerStateMachine.idle);
                    }
                    else if (player.InvokeFunc(player.IsTouchWall) && playerStateMachine.xInput == player.InvokeFunc(player.CheckFacingDir))
                    {
                        stateMachine.ChangeState(playerStateMachine.wallSlide);
                    }
                }
            }
        }
    }
}
