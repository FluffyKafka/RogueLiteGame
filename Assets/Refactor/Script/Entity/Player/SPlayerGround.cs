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
                public SPlayerGround(CEntityStateMachine _stateMachine, AEntity _entity) : base(_stateMachine, _entity)
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
