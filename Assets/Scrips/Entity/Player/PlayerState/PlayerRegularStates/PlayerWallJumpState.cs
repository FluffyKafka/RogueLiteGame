using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(PlayerStateMachine _stateMachine, Player _Player, string _animBoolName) : base(_stateMachine, _Player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(player.wallJumpHorizontalSpeed * -player.facingDir, player.jumpSpeed);
        Debug.Log("EnterWallJump");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.anim.SetFloat("yVelocity", rg.velocity.y);
        if (rg.velocity.y < 0)
        {
            stateMachine.ChangeState(player.fallState);
        }
    }
}
