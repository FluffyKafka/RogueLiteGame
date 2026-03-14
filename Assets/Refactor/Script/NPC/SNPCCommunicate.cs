using DialogSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCSystem
{
    internal class SNPCCommunicate : SNPCStateBase
    {
        [SerializeField] List<DDialog> dialogs;
        [SerializeField] ENPCStateType nextType;
        [SerializeField] bool isInteractFinishAfterThisState = false;

        protected DDialog dialog;

        public override void Init(CNPCStateMachine _stateMachine, ANPC _npc)
        {
            base.Init(_stateMachine, _npc);
            dialog = dialogs[Random.Range(0, dialogs.Count)];
            
        }

        public override void Enter()
        {
            base.Enter();
            npc.InvokeAction(npc.SetDialogIndexNotice, dialog);
            npc.ToCommunicate(dialog);
            npc.CommunicateFinishNotice += CommunicateFinish;
        }

        public override void Exit()
        {
            base.Exit();
            npc.CommunicateFinishNotice -= CommunicateFinish;
        }

        public override void Update()
        {
            base.Update();
        }

        protected void CommunicateFinish()
        {
            stateMachine.ChangeState(nextType);
            if(isInteractFinishAfterThisState)
            {
                npc.InteractFinish();
            }
        }
    }
}

