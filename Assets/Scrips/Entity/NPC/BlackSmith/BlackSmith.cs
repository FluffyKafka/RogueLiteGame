using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSmith : NPC, IPlayerCommunicable
{
    #region States
    public BlackSmith_IdleState idleState { get; private set; }
    public BlackSmith_TalkingState talkState { get; private set; }
    #endregion

    [Header("Communications")]
    [SerializeField] public List<Dialog> Communicate;
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
            stateMachine.ChangeState(talkState);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        idleState = new BlackSmith_IdleState(this, stateMachine, "Idle", this);
        talkState = new BlackSmith_TalkingState(this, stateMachine, "Idle", this);
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
