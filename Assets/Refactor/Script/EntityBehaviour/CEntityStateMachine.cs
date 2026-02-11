using UnityEngine;

namespace EntityBehaviour
{
    internal abstract class CEntityStateMachine : CEntityComponentBase
    {
        protected SEntityState currentState;
        protected bool haveStateChangeInThisUpdate = false;
        protected bool isDenyStateChange = false;

        protected override void Awake()
        {
            base.Awake();
            entity.Die += Die;
        }

        protected void Initialize(SEntityState _startState)
        {
            currentState = _startState;
            currentState.Enter();
        }

        public virtual void ChangeState(SEntityState _newState)
        {
            if (isDenyStateChange)
            {
                return;
            }

            if (!haveStateChangeInThisUpdate)
            {
                currentState.Exit();
                currentState = _newState;
                currentState.Enter();
                haveStateChangeInThisUpdate = true;                
            }
        }
        protected void StateChangeDebugMessage(SEntityState _newState)
        {
            Debug.Log("current state is: " + currentState.GetType().Name);
            Debug.Log("next state is: " + _newState.GetType().Name);
            Debug.Log("//////////////////////////////////////////////////////////");
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

        protected abstract void Die();
    }
}

