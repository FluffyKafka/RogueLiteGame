using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter { get; private set; } = 0;
    private float lastAttackTime;

    public PlayerPrimaryAttackState(PlayerStateMachine _stateMachine, Player _Player, string _animBoolName) : base(_stateMachine, _Player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (Time.time - lastAttackTime > player.comboWindow)
        {
            comboCounter = 0;
        }
        player.anim.SetInteger("ComboCounter", comboCounter);
        player.anim.speed = player.GetStats().GetStatByType(StatType.AttackSpeed) / 100;
        

        float attackDir = player.facingDir;
        if(xInput != 0)
        {
            attackDir = xInput;
        }
        player.SetVelocity(player.attackMovement[comboCounter] * attackDir, rg.velocity.y);

        timer = player.movableDurationInAttacking;
    }

    public override void Exit()
    {
        base.Exit();
        player.anim.speed = 1;
        ++comboCounter;
        comboCounter %= 3;
        lastAttackTime = Time.time;

        player.StartCoroutine("BusyFor", player.unmovableDurationAfterAttack);
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("PrimaryAttack");
        if (timer < 0)
        {
            player.SetVelocity(0, 0);
        }
        if (isAnimFinish)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
