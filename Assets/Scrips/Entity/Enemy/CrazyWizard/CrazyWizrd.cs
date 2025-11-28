using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyWizrd : Enemy
{
    #region States
    public EnemyState idleState { get; private set; }
    public EnemyState wanderState { get; private set; }
    public EnemyState battleIdleState { get; private set; }
    public EnemyState battleMoveState { get; private set; }
    public EnemyState attackState { get; private set; }
    public EnemyState stunnedState { get; private set; }
    public EnemyState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new CrazyWizard_IdleState(this, stateMachine, "Idle", this);
        wanderState = new CrazyWizard_WanderState(this, stateMachine, "Move", this);
        battleIdleState = new CrazyWizard_BattleIdleState(this, stateMachine, "Idle", this);
        battleMoveState = new CrazyWizard_BattleMoveState(this, stateMachine, "Move", this);
        attackState = new CrazyWizard_AttackState(this, stateMachine, "Attack", this);
        stunnedState = new CrazyWizard_StunnedState(this, stateMachine, "Stunned", this);
        deadState = new CrazyWizard_DeadState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        overlapCheckRadius = GetComponent<CapsuleCollider2D>().size.x;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool TryToBeStuuned()
    {
        if (base.TryToBeStuuned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

    public override void DamageSourceNotice(Entity _damageSource)
    {
        if (_damageSource != null)
        {
            Player player = _damageSource as Player;
            if (player != null && stateMachine.currentState == idleState || stateMachine.currentState == wanderState)
            {
                stateMachine.ChangeState(battleIdleState);
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
}
