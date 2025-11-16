using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader_BusyState : TraderStateBase
{
    public Trader_BusyState(NPC _npcBase, NPCStateMachine _npcStateMachine, string _animBoolName, Trader _npc) : base(_npcBase, _npcStateMachine, _animBoolName, _npc)
    {
    }

    public override void Enter()
    {
        base.Enter();
        GameManager.instance.SetPauseGame(true);
        npc.anim.speed *= GameManager.instance.pauseTimeSpeedDivider;
        PlayerManager.instance.player.anim.speed *= GameManager.instance.pauseTimeSpeedDivider;
    }

    public override void Exit()
    {
        base.Exit();
        GameManager.instance.SetPauseGame(false);
        npc.anim.speed /= GameManager.instance.pauseTimeSpeedDivider;
        PlayerManager.instance.player.anim.speed /= GameManager.instance.pauseTimeSpeedDivider;
    }

    public override void Update()
    {
        base.Update();
    }
}
