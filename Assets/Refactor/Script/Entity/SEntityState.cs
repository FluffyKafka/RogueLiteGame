using EntitySystem.EntityActor;
using EntitySystem.EntityActor.PlayerActor;
using EntitySystem.EntityComponent.StateMachineComponent;
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
            protected string animName;

            public SEntityState(CEntityStateMachine _stateMachine, AEntity _entity, string _animName)
            {
                this.stateMachine = _stateMachine;
                entity = _entity;
                animName = _animName;
            }

            public virtual void Enter()
            {
                entity.StateChange?.Invoke();
            }

            public virtual void Update()
            {

            }

            public virtual void Exit()
            {

            }

            public virtual string CheckAnimName()
            {
                return animName;
            }
        }
    }
}