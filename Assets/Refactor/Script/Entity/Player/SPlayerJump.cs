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
                    player.InvokeAction(player.Jump);
                    isFinishJump = false;
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
                        playerStateMachine.ChangeState(playerStateMachine.fall);
                        return;
                    }

                    if (player.InvokeFunc(player.IsGroundedOrPlatForm))
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