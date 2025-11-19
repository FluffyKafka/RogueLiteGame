using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : NPC, IPlayerCommunicable
{
    #region States
    public Trader_IdleState idleState {  get; private set; }
    public Trader_BeforeTradeState beforeTradeState { get; private set; }
    public Trader_TradeState tradeState { get; private set; }
    public Trader_AfterTradeState afterTradeState { get; private set; }
    #endregion

    [Header("Communications")]
    [SerializeField] public List<Dialog> beforeTrade;
    [SerializeField] public List<Dialog> afterTrade;
    [SerializeField] public List<Dialog> successDialogs;
    [SerializeField] public List<Dialog> noCoinDialogs;
    [SerializeField] public List<Dialog> fullBagDialogs;
    [SerializeField] public List<ItemData> possibleItems;
    [SerializeField] public int productAmount;
    [HideInInspector] public List<ItemData> products = new List<ItemData>();

    [HideInInspector] public List<Sentence> currentDialog;
    [HideInInspector] public int sentenceIndex = 0;

    public void Talk(Player _player)
    {
        if (PlayerManager.instance.player.enemyCheck.isBattle)
        {
            PlayerManager.instance.player.fx.CreatePopUpText("战斗时无法对话");
            return;
        }

        if (stateMachine.currentState == idleState)
        {
            stateMachine.ChangeState(beforeTradeState);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        idleState = new Trader_IdleState(this, stateMachine, "Idle", this);
        beforeTradeState = new Trader_BeforeTradeState(this, stateMachine, "Idle", this);
        tradeState = new Trader_TradeState(this, stateMachine, "Trade", this);
        afterTradeState = new Trader_AfterTradeState(this, stateMachine, "Idle", this);
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
