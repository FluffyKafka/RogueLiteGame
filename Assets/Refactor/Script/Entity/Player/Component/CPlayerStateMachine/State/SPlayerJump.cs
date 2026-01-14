using EntitySystem.EntityActor;
using EntitySystem.EntityComponent.StateMachineComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EntitySystem
{
    namespace EntityState
    {
        namespace PlayerState
        {
            internal class SPlayerJump : SPlayerAir
            {
                protected bool isFinishJump = false;
                public SPlayerJump(CEntityStateMachine _stateMachine, AEntity _entity, string _animName) : base(_stateMachine, _entity, _animName)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                    if(player.Jump == null)
                    {
                        Debug.LogWarning("Jump服务未提供，检查运动组件");
                    }
                    player.Jump?.Invoke();
                    isFinishJump = false;
                }

                public override void Exit()
                {
                    base.Exit();
                }

                public override void Update()
                {
                    base.Update();
                    Assert.IsTrue(player.IsFall != null, "缺少IsFall服务提供者，检查运动组件是否正确");
                    if(player.IsFall.Invoke())
                    {
                        playerStateMachine.ChangeState(playerStateMachine.fall);
                        return;
                    }

                    Assert.IsNotNull(player.IsGrounded, "IsGrounded服务未提供，检查碰撞系统");
                    if (player.IsGrounded.Invoke())
                    {
                        if(isFinishJump)
                        {
                            playerStateMachine.ChangeState(playerStateMachine.idle);
                        }
                    }
                    else
                    {
                        if(!isFinishJump)
                        {
                            isFinishJump = true;
                        }
                    }
                }
            }
        }
    }
}
