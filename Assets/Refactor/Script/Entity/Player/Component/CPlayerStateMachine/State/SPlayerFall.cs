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
            internal class SPlayerFall : SPlayerAir
            {
                public SPlayerFall(CEntityStateMachine _stateMachine, AEntity _entity, string _animName) : base(_stateMachine, _entity, _animName)
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
                    Assert.IsNotNull(player.IsGrounded, "IsGrounded服务未提供，检查碰撞系统");
                    if (player.IsGrounded.Invoke())
                    {
                        playerStateMachine.ChangeState(playerStateMachine.idle);
                    }
                }
            }
        }
    }
}
