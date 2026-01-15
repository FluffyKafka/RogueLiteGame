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
            internal class SPlayerWallJump : SPlayerAir
            {
                public SPlayerWallJump(CEntityStateMachine _stateMachine, AEntity _entity, string _animName) : base(_stateMachine, _entity, _animName)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                    player.InvokeAction(player.WallJump);
                    canMove = false;
                }

                public override void Exit()
                {
                    base.Exit();
                }

                public override void Update()
                {
                    base.Update();
                    if (player.InvokeFunc(player.IsFall))
                    {
                        stateMachine.ChangeState(playerStateMachine.fall);
                    }
                }
            }
        }
    }
}