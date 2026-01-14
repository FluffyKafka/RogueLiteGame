using EntitySystem.EntityActor;
using EntitySystem.EntityActor.PlayerActor;
using EntitySystem.EntityComponent.StateMachineComponent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace EntitySystem
{
    namespace EntityState
    {
        namespace PlayerState
        {
            internal class SPlayerState : SEntityState
            {
                protected APlayer player;
                protected CPlayerStateMachine playerStateMachine;

                public SPlayerState(CEntityStateMachine _stateMachine, AEntity _entity, string _animName) : base(_stateMachine, _entity, _animName)
                {
                    Assert.IsTrue(_entity is APlayer, "此状态属于Player专用状态，不能被用于控制其他Entity");
                    player = _entity as APlayer;
                    Assert.IsTrue(_stateMachine is CPlayerStateMachine, "此状态属于Player专用状态，只能被Player状态机持有");
                    playerStateMachine = _stateMachine as CPlayerStateMachine;
                }

                override public void Enter()
                {
                    base.Enter();
                }

                override public void Update()
                {
                    base.Update();
                }

                override public void Exit()
                {
                    base.Exit();
                }
            }
        }
    }
}

