using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(PlayerStateMachine _stateMachine, Player _Player, string _animBoolName) : base(_stateMachine, _Player, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, 0);
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("Fall");
        if (player.IsGrounded() || player.IsPlatform())
        {
            stateMachine.ChangeState(player.idleState);
        }

    }
}
