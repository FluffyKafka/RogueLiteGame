using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witcher_SpellState : Witcher_BusyState
{
    private bool isSpell;
    public Witcher_SpellState(NPC _npcBase, NPCStateMachine _npcStateMachine, string _animBoolName, Witcher _npc) : base(_npcBase, _npcStateMachine, _animBoolName, _npc)
    {
    }

    public override void Enter()
    {
        base.Enter();

        isSpell = UI.instance.TryShowSkillLearningBlock();

        if (!isSpell)
        {
            npc.sentenceIndex = 0;
            npc.currentDialog = npc.noneSpell[UnityEngine.Random.Range(0, npc.noneSpell.Count)].sentences;

            UI.instance.Speak(npc.currentDialog[npc.sentenceIndex]);
            ++npc.sentenceIndex;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (isSpell)
        {
            if(UI.instance.IsSkillLearningBlockHide())
            {
                stateMachine.ChangeState(npc.afterSpellState);
            }
            else if(Input.GetKeyDown(KeyCode.Escape))
            {
                stateMachine.ChangeState(npc.idleState);
            }
        }
        else
        {
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
                    stateMachine.ChangeState(npc.finishState);
                }
            }
        }
    }
}
