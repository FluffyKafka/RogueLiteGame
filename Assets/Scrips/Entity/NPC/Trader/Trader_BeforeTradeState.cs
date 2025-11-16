using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader_BeforeTradeState : Trader_BusyState
{
    public Trader_BeforeTradeState(NPC _npcBase, NPCStateMachine _npcStateMachine, string _animBoolName, Trader _npc) : base(_npcBase, _npcStateMachine, _animBoolName, _npc)
    {
    }

    public override void Enter()
    {
        base.Enter();
        npc.sentenceIndex = 0;
        npc.currentDialog = npc.beforeTrade[UnityEngine.Random.Range(0, npc.beforeTrade.Count)].sentences;

        UI.instance.Speak(npc.currentDialog[npc.sentenceIndex]);
        ++npc.sentenceIndex;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (npc.sentenceIndex < npc.currentDialog.Count)
            {
                UI.instance.Speak(npc.currentDialog[npc.sentenceIndex]);
                ++npc.sentenceIndex;
            }
            else
            {
                UI.instance.SpeakDone();
                stateMachine.ChangeState(npc.tradeState);
            }
        }
    }
}
