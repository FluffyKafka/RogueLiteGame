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
            internal class SPlayerAir : SPlayerState
            {
                protected bool canMove = true;
                public SPlayerAir(CEntityStateMachine _stateMachine, AEntity _entity, string _animName) : base(_stateMachine, _entity, _animName)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                    player.JumpInput += Jump;
                    player.AttackInput += Attack;
                }

                public override void Exit()
                {
                    base.Exit();
                    player.JumpInput -= Jump;
                    player.AttackInput -= Attack;
                }

                public override void Update()
                {
                    base.Update();
                    player.InvokeAction(player.UpdateAnimYVelocity, player.InvokeFunc(player.CheckYVelocity));
                    if(canMove)
                    {
                        player.InvokeAction(player.Move, playerStateMachine.xInput);
                    }
                }

                protected void Jump()
                {
                    playerStateMachine.ChangeState(playerStateMachine.jump);
                }
                protected void Attack()
                {
                    playerStateMachine.ChangeState(playerStateMachine.primaryAttack);
                }
            }
        }
    }
}

