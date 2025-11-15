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
        Debug.Log("EnterWallSlide");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (yInput > 0 || yInput == 0)
        {
            player.SetVelocityWithoutFlip(rg.velocity.x, -player.wallSlideSpeed + player.wallSlideUpAdjustSpeed);
        }
        else
        {
            player.SetVelocityWithoutFlip(rg.velocity.x, -player.wallSlideSpeed + yInput * player.wallSlideDownAdjustSpeed);
        }

        if (player.CheckInput_KeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
        }
        else if(player.IsGrounded() || !player.IsTouchWall())
        {
            stateMachine.ChangeState(player.idleState);
        }
        else if (xInput != player.facingDir)
        {
            stateMachine.ChangeState(player.fallState);
        }
    }
}
