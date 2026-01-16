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
            internal class SPlayerDead : SPlayerAir
            {
                public SPlayerDead(CEntityStateMachine _stateMachine, AEntity _entity, string _animName) : base(_stateMachine, _entity, _animName)
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
                }
            }
        }
    }
}
