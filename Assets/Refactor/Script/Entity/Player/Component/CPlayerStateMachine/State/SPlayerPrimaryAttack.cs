using EntitySystem.EntityActor;
using EntitySystem.EntityComponent.StateMachineComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
namespace EntitySystem
{
    namespace EntityState
    {
        namespace PlayerState
        {
            internal class SPlayerPrimaryAttack : SPlayerState
            {
                public SPlayerPrimaryAttack(CEntityStateMachine _stateMachine, AEntity _entity, string _animName) : base(_stateMachine, _entity, _animName)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                    player.InvokeAction(player.AttackRaw);
                    player.AttackFinish += OnAttackFinish;
                }

                public override void Exit()
                {
                    base.Exit();
                    playerStateMachine.BusyFor(player.InvokeFunc(player.CheckUnmovableDurationAfterAttack));
                    player.AttackFinish -= OnAttackFinish;
                }

                public override void Update()
                {
                    base.Update();
                }

                protected void OnAttackFinish()
                {
                    playerStateMachine.ChangeState(playerStateMachine.idle);
                }
            }
        }
    }
}