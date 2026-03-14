using EntityBehaviour;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace NPCSystem
{
    internal enum ENPCStateType
    {
        Idle,
        BeforeEffectCommunicate,
        AfterEffectCommunicate,
        EffectFailCommunicate,
        Effect
    }
    internal class CNPCStateMachine : CNPCComponentBase
    {
        [SerializeField] protected SNPCStateBase initState;

        protected Dictionary<ENPCStateType, SNPCStateBase> stateTypeMap = new();
        protected SNPCStateBase currentState;
        protected bool haveStateChangeInThisUpdate = false;
        protected bool isDenyStateChange = false;

        protected override void Awake()
        {
            base.Awake();
        }

        protected void Start()
        {
            SNPCStateBase[] states = GetComponents<SNPCStateBase>();
            foreach(var state in states)
            {
                stateTypeMap.Add(state.CheckType(), state);
                state.Init(this, npc);
            }
            Initialize(initState);
        }
        protected void Initialize(SNPCStateBase _startState)
        {
            currentState = _startState;
            currentState.Enter();
        }

        public virtual void ChangeState(ENPCStateType _newState)
        {
            if (isDenyStateChange)
            {
                return;
            }

            if (!haveStateChangeInThisUpdate)
            {
                currentState.Exit();
                currentState = stateTypeMap[_newState];
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

        protected void LateUpdate()
        {
            haveStateChangeInThisUpdate = false;
        }
    }

    internal class SNPCStateBase : MonoBehaviour
    {
        [SerializeField] protected ENPCStateType type;

        protected CNPCStateMachine stateMachine;
        protected ANPC npc;
        public ENPCStateType CheckType()
        {
            return type;
        }
        public virtual void Init(CNPCStateMachine _stateMachine, ANPC _npc)
        {
            stateMachine = _stateMachine;
            npc = _npc;
        }

        public virtual void Enter()
        {
            enabled = true;
        }

        public virtual void Exit()
        {
            enabled = false;
        }

        public virtual void Update()
        {

        }
    }
}

