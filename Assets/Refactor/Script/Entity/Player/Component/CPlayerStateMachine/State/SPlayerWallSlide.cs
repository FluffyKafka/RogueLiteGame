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
            internal class SPlayerWallSlide : SPlayerState
            {

                public SPlayerWallSlide(CEntityStateMachine _stateMachine, AEntity _entity, string _animName) : base(_stateMachine, _entity, _animName)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                    player.JumpInput += OnJump;
                }

                public override void Exit()
                {
                    base.Exit();
                    player.JumpInput -= OnJump;
                }

                public override void Update()
                {
                    base.Update();
                    player.WallSlide?.Invoke(playerStateMachine.yInput);

                    bool IsGroundedOrPlatForm = player.InvokeFunc(player.IsGroundedOrPlatForm);
                    bool isTouchWall = player.InvokeFunc(player.IsTouchWall);
                    int facingDir = player.InvokeFunc(player.CheckFacingDir);

                    if (IsGroundedOrPlatForm || !isTouchWall)
                    {
                        stateMachine.ChangeState(playerStateMachine.idle);
                    }
                    else if (playerStateMachine.xInput != facingDir)
                    {
                        stateMachine.ChangeState(playerStateMachine.fall);
                    }
                }

                public void OnJump()
                {
                    playerStateMachine.ChangeState(playerStateMachine.wallJump);
                }

            }
        }
    }
}