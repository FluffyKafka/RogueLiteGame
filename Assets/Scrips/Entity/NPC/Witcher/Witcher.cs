using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Witcher : NPC, IPlayerCommunicable
{
    #region States
    public Witcher_IdleState idleState {  get; private set; }
    public Witcher_BeforeSpellState beforeSpellState { get; private set; }
    public Witcher_SpellState spellState { get; private set; }
    public Witcher_AfterSpellState afterSpellState { get; private set; }
    public Witcher_FinishState finishState { get; private set; }
    #endregion

    [Header("Communications")]
    [SerializeField] public List<Dialog> beforeSpell;
    [SerializeField] public List<Dialog> afterSpell;
    [SerializeField] public List<Dialog> noneSpell;

    [HideInInspector] public List<Sentence> currentDialog;
    [HideInInspector] public int sentenceIndex = 0;

    public void Talk(Player _player)
    {
        if(PlayerManager.instance.player.enemyCheck.isBattle)
        {
            PlayerManager.instance.player.fx.CreatePopUpText("战斗时无法对话");
            return;
        }

        if(stateMachine.currentState == idleState)
        {
            stateMachine.ChangeState(beforeSpellState);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        idleState = new Witcher_IdleState(this, stateMachine, "Idle", this);
        beforeSpellState = new Witcher_BeforeSpellState(this, stateMachine, "Idle", this);
        spellState = new Witcher_SpellState(this, stateMachine, "Spell", this);
        afterSpellState = new Witcher_AfterSpellState(this, stateMachine, "Idle", this);
        finishState = new Witcher_FinishState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

}
