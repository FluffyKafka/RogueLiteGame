using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    private float afterImageGenerateTimer;
    public PlayerDashState(PlayerStateMachine _stateMachine, Player _Player, string _animBoolName) : base(_stateMachine, _Player, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        timer = player.dashDuration;
        player.isDashing = true;
        player.skill.dash.TryCreateClone_DashStart();
        SceneAudioManager.instance.playerSFX.dash.Play(null);
        afterImageGenerateTimer = player.fx.afterImageGenerateCooldown;
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, rg.velocity.y);
        player.isDashing = false;
        player.skill.dash.TryCreateClone_DashEnd();
    }

    public override void Update()
    { 
        base.Update();
        Debug.Log("Dash");
        player.SetVelocity(player.dashSpeed * player.dashDir, 0);

        afterImageGenerateTimer -= Time.deltaTime;
        if(afterImageGenerateTimer < 0)
        {
            player.fx.CreateAfterImage(player.isFacingLeft);
            afterImageGenerateTimer = player.fx.afterImageGenerateCooldown;
        }

        if (timer < 0 || player.IsTouchWall())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
