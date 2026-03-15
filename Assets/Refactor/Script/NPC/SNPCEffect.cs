using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCSystem
{
    internal class SNPCEffect : SNPCStateBase
    {
        [SerializeField] protected bool haveAfterEffectDialog = true;

        public override void Init(CNPCStateMachine _stateMachine, ANPC _npc)
        {
            base.Init(_stateMachine, _npc);
        }

        public override void Enter()
        {
            base.Enter();
            if (haveAfterEffectDialog)
            {
                npc.EffectFinishNotice += EffectFinish;
                npc.EffectFailNotice += EffectFail;
            }
            else
            {
                npc.EffectFinishNotice += NoAfterEffectDialogFinish;
                npc.EffectFailNotice += NoAfterEffectDialogFinish;
            }
            StartCoroutine(EffectDelay());
        }
        protected IEnumerator EffectDelay()
        {
            yield return null;
            npc.InvokeAction(npc.EffectNotice);
        }

        public override void Exit()
        {
            base.Exit();
            if (haveAfterEffectDialog)
            {
                npc.EffectFinishNotice -= EffectFinish;
                npc.EffectFailNotice -= EffectFail;
            }
            else
            {
                npc.EffectFinishNotice -= NoAfterEffectDialogFinish;
                npc.EffectFailNotice -= NoAfterEffectDialogFinish;
            }
        }

        public override void Update()
        {
            base.Update();
        }

        protected void EffectFinish()
        {
            stateMachine.ChangeState(ENPCStateType.AfterEffectCommunicate);
        }
        protected void EffectFail()
        {
            stateMachine.ChangeState(ENPCStateType.EffectFailCommunicate);
        }
        protected void NoAfterEffectDialogFinish()
        {
            stateMachine.ChangeState(ENPCStateType.Idle);
            npc.InteractFinish();
        }
    }
}

