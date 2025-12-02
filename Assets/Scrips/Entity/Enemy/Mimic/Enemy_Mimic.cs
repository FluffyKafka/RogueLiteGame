using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class Enemy_Mimic : Enemy, IPlayerInteractive
{
    #region Components
    public Mimic_ChestState chestState { get; private set; }
    public Mimic_OpenState openState { get; private set; }
    public Mimic_WatchState watchState { get; private set; }
    public Mimic_BattleIdleState battleIdleState { get; private set; }
    public Mimic_MoveState moveState { get; private set; }
    public Mimic_BattleState battleState { get; private set; }
    public Mimic_IdleState idleState { get; private set; }
    public Mimic_AttackState attackState { get; private set; }
    public Mimic_StunState stunState { get; private set; }
    public Mimic_DeadState deadState { get; private set; }
    #endregion
    [Header("Mimic UI Hide")]
    [SerializeField] public GameObject enemyUI;

    [Header("Mimic or Chest Info")]
    [Range(0, 100)][SerializeField] private float chestRate;
    [HideInInspector] public bool isChest;

    [Header("Mimic WatchState Info")]
    [SerializeField] public float watchCooldown;
    [Range(0, 100)][SerializeField] public float watchRate;

    protected override void Awake()
    {
        base.Awake();
        chestState = new Mimic_ChestState(this, stateMachine, "Chest", this);
        openState = new Mimic_OpenState(this, stateMachine, "Open", this);
        watchState = new Mimic_WatchState(this, stateMachine, "Watch", this);
        battleIdleState = new Mimic_BattleIdleState(this, stateMachine, "BattleIdle", this);
        moveState = new Mimic_MoveState(this, stateMachine, "Move", this);
        battleState = new Mimic_BattleState(this, stateMachine, "Move", this);
        idleState = new Mimic_IdleState(this, stateMachine, "BattleIdle", this);
        attackState = new Mimic_AttackState(this, stateMachine, "Attack", this);
        stunState = new Mimic_StunState(this, stateMachine, "Stun", this);
        deadState = new Mimic_DeadState(this, stateMachine, "BattleIdle", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(chestState);
        overlapCheckRadius = GetComponent<CapsuleCollider2D>().size.x;

        //ÆäÎª±¦Ïä
        if(Random.Range(0, 100) < chestRate)
        {
            isChest = true;
            SetCanBeDamage(false);
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool TryToBeStuuned()
    {
        if (base.TryToBeStuuned())
        {
            stateMachine.ChangeState(stunState);
            return true;
        }
        return false;
    }

    public override void DamageSourceNotice(Entity _damageSource)
    {
        if (_damageSource != null)
        {
            Player player = _damageSource as Player;
            if (player != null && ((stateMachine.currentState == chestState && !isChest) || stateMachine.currentState == watchState))
            {
                stateMachine.ChangeState(battleIdleState);
                enemyUI.SetActive(true);
            }
        }
    }

    public override void Die()
    {
        if (!isDead)
        {
            stateMachine.ChangeState(deadState);
            base.Die();
        }
    }

    public override void Flip()
    {
        base.Flip();
    }

    public void Interact()
    {
        if(GetComponentInChildren<PopUpTextComponent>() != null)
        {
            GetComponentInChildren<PopUpTextComponent>().FinishPopUpText();
            Destroy(GetComponentInChildren<PopUpTextComponent>());
        }

        if(isChest)
        {
            stateMachine.ChangeState(openState);

            SceneAudioManager.instance.mimicSFX.open.Play(transform);
        }
        else
        {
            stateMachine.ChangeState(battleIdleState);
            enemyUI.SetActive(true);

            SceneAudioManager.instance.mimicSFX.roar.Play(transform);
        }
    }
}
