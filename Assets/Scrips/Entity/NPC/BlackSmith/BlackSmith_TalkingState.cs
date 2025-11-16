using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSmith_TalkingState : BlackSmithStateBase
{
    public BlackSmith_TalkingState(NPC _npcBase, NPCStateMachine _npcStateMachine, string _animBoolName, BlackSmith _npc) : base(_npcBase, _npcStateMachine, _animBoolName, _npc)
    {
    }

    public override void Enter()
    {
        base.Enter();
        npc.sentenceIndex = 0;
        npc.currentDialog = npc.Communicate[UnityEngine.Random.Range(0, npc.Communicate.Count)].sentences;

        UI.instance.Speak(npc.currentDialog[npc.sentenceIndex]);
        ++npc.sentenceIndex;

        GameManager.instance.SetPauseGame(true);
        npc.anim.speed *= GameManager.instance.pauseTimeSpeedDivider;
        PlayerManager.instance.player.anim.speed *= GameManager.instance.pauseTimeSpeedDivider;
    }

    public override void Exit()
    {
        base.Exit();
        UI.instance.SpeakDone();

        GameManager.instance.SetPauseGame(false);
        npc.anim.speed /= GameManager.instance.pauseTimeSpeedDivider;
        PlayerManager.instance.player.anim.speed /= GameManager.instance.pauseTimeSpeedDivider;
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
                UI.instance.SwitchTo(UI.instance.craftUI);
                stateMachine.ChangeState(npc.idleState);
            }
        }
    }
}
