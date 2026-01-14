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
            internal class SPlayerMove : SPlayerGround
            {
                protected float xInput = 0;

                public SPlayerMove(CEntityStateMachine _stateMachine, AEntity _entity, string _animName) : base(_stateMachine, _entity, _animName)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                    player.HorizonInput += HorizonInputCheck;
                }

                public override void Exit()
                {
                    base.Exit();
                    player.HorizonInput -= HorizonInputCheck;
                }

                public override void Update()
                {
                    base.Update();

                    player.Move?.Invoke(xInput);
                    if(xInput == 0)
                    {
                        playerStateMachine.ChangeState(playerStateMachine.idle);
                    }
                    xInput = 0;
                }

                public void HorizonInputCheck(float _xInput)
                {
                    xInput = _xInput;
                }
            }
        }
    }
}
