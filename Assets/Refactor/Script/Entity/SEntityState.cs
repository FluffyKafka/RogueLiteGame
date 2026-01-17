using EntitySystem.EntityActor;
using EntitySystem.EntityActor.PlayerActor;
using EntitySystem.EntityComponent.StateMachineComponent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EntitySystem
{
    namespace EntityState
    {
        internal class SEntityState
        {
            protected CEntityStateMachine stateMachine;
            protected AEntity entity;

            public SEntityState(CEntityStateMachine _stateMachine, AEntity _entity)
            {
                this.stateMachine = _stateMachine;
                entity = _entity;
            }

            public virtual void Enter()
            {
                
            }

            public virtual void Update()
            {

            }

            public virtual void Exit()
            {

            }
        }
    }
}