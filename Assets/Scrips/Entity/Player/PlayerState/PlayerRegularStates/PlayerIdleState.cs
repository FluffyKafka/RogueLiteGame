using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerStateMachine _stateMachine, Player _Player, string _animBoolName) : base(_stateMachine, _Player, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0, rg.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        Debug.Log("Idle");
        player.SetVelocity(0, rg.velocity.y);
        base.Update();
        if (xInput != 0 && !(xInput == player.facingDir && player.IsTouchWall()) && !player.isBusy)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }
}
