using EntitySystem.EntityActor;
using EntitySystem.EntityState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EntitySystem
{
    namespace EntityComponent
    {
        namespace StateMachineComponent
        {
            internal abstract class CEntityStateMachine: CEntityComponentBase
            {
                protected SEntityState currentState;
                protected bool haveStateChangeInThisUpdate = false;
                protected bool isDenyStateChange = false;

                protected override void Awake()
                {
                    base.Awake();
                    entity.CheckStateAnimName += CheckCurrentStateAnimName;
                    entity.Die += Die;
                }

                protected void Initialize(SEntityState _startState)
                {
                    currentState = _startState;
                    currentState.Enter();
                }

                public virtual void ChangeState(SEntityState _newState)
                {
                    if(isDenyStateChange)
                    {
                        return;
                    }    

                    if(!haveStateChangeInThisUpdate)
                    {
                        currentState.Exit();
                        currentState = _newState;
                        currentState.Enter();
                        haveStateChangeInThisUpdate = true;
                    }
                }

                protected override void Update()
                {
                    currentState.Update();
                }

                virtual protected void Start()
                {

                }

                protected void LateUpdate()
                {
                    haveStateChangeInThisUpdate = false;
                }

                protected string CheckCurrentStateAnimName()
                {
                    return currentState.CheckAnimName();
                }

                protected abstract void Die();
            }
        }
    }
}

