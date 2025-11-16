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
        if (player.CheckInput_KeyDown(KeyCode.Space) && player.jumpCount < player.jumpCountMax)
        {
            stateMachine.ChangeState(player.jumpState);
        }

        if (player.CheckInput_KeyDown(KeyCode.Mouse0))
        {
            if (!player.IsSwordThrown())
            {
                stateMachine.ChangeState(player.primaryAttackState);
            }
            else
            {
                player.swordThrown.GetComponent<SwordSkillController>().ReturnSword();
            }
        }

        if (player.CheckInput_KeyDown(KeyCode.E) && SkillManager.intance.counterAttack.isUnlocked_counterAttack && SkillManager.intance.counterAttack.TryUseSkill() && !player.isKnocked)
        {
            stateMachine.ChangeState(player.counterAttackState);
        }

        if (player.CheckInput_KeyDown(KeyCode.Mouse1) && SkillManager.intance.swordThrow.isUnlocked_sword)
        {
            if (!player.IsSwordThrown())
            {
                if (SkillManager.intance.swordThrow.IsOutCooldown())
                {
                    stateMachine.ChangeState(player.aimSwordState);
                }
            }
            else
            {
                player.swordThrown.GetComponent<SwordSkillController>().ReturnSword();
            }
        }

        if (player.CheckInput_KeyDown(KeyCode.Q) && SkillManager.intance.blackHole.isUnlocked_blackHole && SkillManager.intance.blackHole.TryUseSkill())
        {
            stateMachine.ChangeState(player.blackHoleState);
        }
    }
}
