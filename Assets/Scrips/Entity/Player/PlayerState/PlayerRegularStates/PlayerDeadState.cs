using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(PlayerStateMachine _stateMachine, Player _Player, string _animBoolName) : base(_stateMachine, _Player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Inventory.instance.DropAllItem();

        GameManager.instance.isPlayerRemainingExist = true;
        GameManager.instance.playerRemainingSceneName = SceneManager.GetActiveScene().name;
        GameManager.instance.playerLeftCurrency = 
            (int)(Random.Range(player.minPlayerCurrencyRemainRate, player.maxPlayerCurrencyRemainRate) * PlayerManager.instance.GetCurrencyAmount());

        PlayerManager.instance.Die();
        UI.instance.Die();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        player.SetVelocity(0, rg.velocity.y);
        base.Update();
    }
}
