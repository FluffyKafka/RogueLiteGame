using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGhost : Enemy
{
    #region States
    public EnemyState idleState { get; private set; }
    public EnemyState flyState { get; private set; }
    public EnemyState battleIdleState { get; private set; }
    public EnemyState battleFlyState { get; private set; }
    public EnemyState stunState { get; private set; }
    public EnemyState attackState { get; private set; }
    public EnemyState deadState { get; private set; }
    #endregion

    [Header("EnemyGhost Info")]
    public float wanderRadius;
    private NavMeshAgent navMeshAgent;
    public float gravityScale = 12;
    

    protected override void Awake()
    {
        base.Awake();

        idleState = new EnemyGhost_IdleState(this, stateMachine, "Idle", this);
        flyState = new EnemyGhost_FlyState(this, stateMachine, "Fly", this);
        battleIdleState = new EnemyGhost_BattleIdleState(this, stateMachine, "Idle", this);
        battleFlyState = new EnemyGhost_BattleFlyState(this, stateMachine, "Fly", this);
        stunState = new EnemyGhost_StunState(this, stateMachine, "Stun", this);
        attackState = new EnemyGhost_AttackState(this, stateMachine, "Attack", this);
        deadState = new EnemyGhost_DeadState(this, stateMachine, "Idle", this);

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
        overlapCheckRadius = GetComponent<CapsuleCollider2D>().size.x;
    }

    public void FlipCheck()
    {
        if (isKnocked)
        {
            return;
        }

        if (navMeshAgent.hasPath)
        {
            if (navMeshAgent.velocity.x > 0 && isFacingLeft)
            {
                Flip();
            }
            else if (navMeshAgent.velocity.x < 0 && !isFacingLeft)
            {
                Flip();
            }
        }
    }

    public override void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
        {
            return;
        }
        navMeshAgent.velocity = new Vector3(_xVelocity, _yVelocity);
        FlipCheck(_xVelocity);
    }

    public override void SetVelocityWhenKnockBack(float _xVelocity, float _yVelocity)
    {
        navMeshAgent.velocity = new Vector2(_xVelocity, _yVelocity);
    }

    public override void SetVelocityWithoutFlip(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
        {
            return;
        }
        navMeshAgent.velocity = new Vector2(_xVelocity, _yVelocity);
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
            if (player != null && stateMachine.currentState is EnemyGhost_WanderState)
            {
                stateMachine.ChangeState(battleIdleState);
            }
        }
    }

    public override void Die()
    {
        if (!isDead)
        {
            navMeshAgent.enabled = false;
            stateMachine.ChangeState(deadState);
            base.Die();
            rg.isKinematic = false;
            rg.gravityScale = gravityScale;
            navMeshAgent.enabled = false;
        }
    }

    public void SetNavMeshAgentToWanderState()
    {
        navMeshAgent.speed = moveSpeed;
        navMeshAgent.stoppingDistance = 0;
    }

    public void SetNavMeshAgentToBattleState()
    {
        navMeshAgent.speed = battleMoveSpeed;
        navMeshAgent.stoppingDistance = attackValidCheckRadius / 2;
    }

    public void SetDestination(Vector3 _target)
    {
        navMeshAgent.SetDestination(_target);
    }

    public bool IsDestinationValid_LastFrame()
    {
        return navMeshAgent.hasPath;
    }

    public void Stop()
    {
        navMeshAgent.ResetPath();
    }


}
