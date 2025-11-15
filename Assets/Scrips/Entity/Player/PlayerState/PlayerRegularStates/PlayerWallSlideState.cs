using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(PlayerStateMachine _stateMachine, Player _Player, string _animBoolName) : base(_stateMachine, _Player, _animBoolName)
    {
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
        Debug.Log("WallSlide");
        if (yInput > 0)
        {
            player.SetVelocity(rg.velocity.x, -player.wallSlideSpeed + yInput * player.wallSlideUpAdjustSpeed);
        }
        else
        {
            player.SetVelocity(rg.velocity.x, -player.wallSlideSpeed + yInput * player.wallSlideDownAdjustSpeed);
        }

        if (player.IsGrounded() || !player.IsTouchWall())
        {
            stateMachine.ChangeState(player.idleState);
        }
        else if (xInput == -player.facingDir)
        {
            stateMachine.ChangeState(player.fallState);
        }
        else if(player.CheckInput_KeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
        }
    }
}
