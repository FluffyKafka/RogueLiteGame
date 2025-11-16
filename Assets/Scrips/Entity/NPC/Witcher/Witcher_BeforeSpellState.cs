using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witcher_BeforeSpellState : Witcher_BusyState
{
    public Witcher_BeforeSpellState(NPC _npcBase, NPCStateMachine _npcStateMachine, string _animBoolName, Witcher _npc) : base(_npcBase, _npcStateMachine, _animBoolName, _npc)
    {
    }

    public override void Enter()
    {
        base.Enter();
        npc.sentenceIndex = 0;
        npc.currentDialog = npc.beforeSpell[UnityEngine.Random.Range(0, npc.beforeSpell.Count)].sentences;

        UI.instance.Speak(npc.currentDialog[npc.sentenceIndex]);
        ++npc.sentenceIndex;
    }

    public override void Exit()
    {
        base.Exit();
        UI.instance.SpeakDone();
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
                stateMachine.ChangeState(npc.spellState);
            }
        }
    }
}
