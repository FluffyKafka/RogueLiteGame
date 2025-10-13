using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSmithStateBase : NPCState
{
    protected BlackSmith npc;
    public BlackSmithStateBase(NPC _npcBase, NPCStateMachine _npcStateMachine, string _animBoolName, BlackSmith _npc) : base(_npcBase, _npcStateMachine, _animBoolName)
    {
        this.npc = _npc;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
