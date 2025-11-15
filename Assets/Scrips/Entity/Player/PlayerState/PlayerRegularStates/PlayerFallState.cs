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
        Debug.Log("EnterFall");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        
        if(player.IsTouchWall() && xInput == player.facingDir)
        {
            stateMachine.ChangeState(player.wallSlideState);
        } 
        else if (player.IsGrounded() || player.IsPlatform())
        {
            stateMachine.ChangeState(player.idleState);
        }

    }
}
