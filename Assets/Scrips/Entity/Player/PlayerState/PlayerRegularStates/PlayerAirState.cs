using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(PlayerStateMachine _stateMachine, Player _Player, string _animBoolName) : base(_stateMachine, _Player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("EnterAir");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.anim.SetFloat("yVelocity", rg.velocity.y);
        if(xInput != 0)
        {
            player.SetVelocity(xInput * player.moveInAirSpeed, rg.velocity.y);
        }
        //if(player.IsTouchWall())
        //{
        //    stateMachine.ChangeState(player.wallSlideState);
        //} 
    }
}
