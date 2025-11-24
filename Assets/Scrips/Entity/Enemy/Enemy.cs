using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Drop
{
    public ItemData item;
    [Range(0f, 100f)] public float dropChance;
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EntityFX))]
[RequireComponent(typeof(DropItem))]
public class Enemy : Entity
{
    [Header("Enemy Difficulty")]
    [SerializeField] public float difficulty;

    #region Move
    [Header("EnemyBase Move Info")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float maxIdleDuration;
    [SerializeField] public float minIdleDuration;
    private float defaultSpeed;
    #endregion

    #region PlayerDetect
    [Header("EnemyBase Player Detect")]
    [SerializeField] public Transform playerCheck;
    [SerializeField] public float playerCheckDistance;
    [SerializeField] public LayerMask whatIsPlayer;
    [SerializeField] public float playerDetectRadius;
    #endregion

    #region Attack
    [Header("EnemyBase Attack Info")]
    [SerializeField] public float toAttackRadius;
    [SerializeField] public float minAttackCooldown;
    [SerializeField] public float maxAttackCooldown;
    [HideInInspector] public float lastAttackTime;
    [SerializeField] public float battleDuration;
    [SerializeField] public float battleMoveSpeed;
    private float defaulltBattleSpeed;
    #endregion

    #region GetHit
    [Header("Enemy Stunned Info")]
    [SerializeField] public float stunDuration;
    [SerializeField] public Vector2 stunDir;
    protected bool canBeStuuned;
    [SerializeField] protected GameObject counterImage;
    #endregion

    [Header("Enemy Overlap Check Info")]
    [SerializeField] protected float overlapCheckRadius = 1.0f;

    [Header("Enemy Dead Animation Info")]
    [SerializeField] public float deadAnimDuration = 0.2f;
    [SerializeField] public Vector2 deadAnimVelocity = new Vector2(0, 10);

    public EnemyStateMachine stateMachine;

    protected DropItem dropItem;

    public string lastAnimBoolName { get; private set; }

    override protected void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine(this);
        defaultSpeed = moveSpeed;
        defaulltBattleSpeed = battleMoveSpeed;
        dropItem = GetComponent<DropItem>();
    }

    protected override void Start()
    {
        base.Start();
        counterImage.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    public void SetDrops(List<Drop> _drops)
    {
        dropItem.SetDrops(_drops);
    }
    public virtual void FreezeTime(bool _isFreeze)
    {
        if(_isFreeze)
        {
            moveSpeed = 0;
            battleMoveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultSpeed;
            anim.speed = 1;
            battleMoveSpeed = defaulltBattleSpeed;
        }
    }

    public virtual IEnumerator FreezeTimerFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }

    public virtual void OpenCounterAttackWindow()
    {
        canBeStuuned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStuuned = false;
        counterImage.SetActive(false);
    }

    public bool CanBeStunned()
    {
        return canBeStuuned;
    }

    public virtual bool TryToBeStuuned()
    {
        if(canBeStuuned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    public virtual RaycastHit2D IsDetectPlayerFront()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDistance, whatIsPlayer);
        if(hit && !hit.collider.GetComponent<Player>().isDead && CanSeePlayer())
        {
            if(hit)
            {
                PlayerManager.instance.player.enemyCheck.SetIsBattle(true);
            }
            return hit;
        }
        else
        {
            return new RaycastHit2D();
        }
    }

    public virtual bool IsPlayerDetected()
    {
        Transform player = PlayerManager.instance.player.transform;
        bool isDetect =
            !PlayerManager.instance.player.isDead && 
            Vector2.Distance(player.position, transform.position) < playerDetectRadius && CanSeePlayer();
        if(isDetect)
        {
            PlayerManager.instance.player.enemyCheck.SetIsBattle(true);
        }
        return isDetect;
    }

    override public void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, playerDetectRadius);
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + playerCheckDistance * facingDir, playerCheck.position.y));
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerCheck.position, toAttackRadius);
        Gizmos.color = Color.white;
    }

    public virtual void AnimFinishTriggerCalled()
    {
        stateMachine.currentState.TriggerCall();
    }

    public bool IsEnemyOverlap()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, overlapCheckRadius);
        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                return true;
            }
        }
        return false;
    }

    public virtual void AssignLastAnimBoolName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }

    public override void SlowEntityBy(float _slowPercent, float _slowDuration)
    {
        base.SlowEntityBy(_slowPercent, _slowDuration);
        moveSpeed *= (1 - _slowPercent);
        battleMoveSpeed *= (1 - _slowPercent);
        Invoke("ResetDefaultSpeed", _slowDuration);
    }

    protected override void ResetDefaultSpeed()
    {
        base.ResetDefaultSpeed();
        moveSpeed = defaultSpeed;
        battleMoveSpeed = defaulltBattleSpeed;
    }

    public override void Die()
    {
        if(!isDead)
        {
            DropItem();
            PlayerManager.instance.AddCurrencyAmount((int)(cs as EnemyStats).currencyDropAmount.GetValue());
            base.Die();
        }
    }
    public void DropItem()
    {
        dropItem.Drop();
    }

    public override CharacterStats GetStats()
    {
        return cs;
    }

    public virtual bool CanAttack()
    {
        return (Time.time - lastAttackTime) > Random.Range(minAttackCooldown, maxAttackCooldown);
    }

    public bool CanSeePlayer()
    {
        return !Physics2D.Linecast(playerCheck.position, PlayerManager.instance.player.transform.position, whatIsGround);
    }

    public void FaceToPlayer()
    {
        if(PlayerManager.instance.player.transform.position.x < transform.position.x)
        {
            if(!isFacingLeft)
            {
                Flip();
            }
        }
        else
        {
            if(isFacingLeft)
            {
                Flip();
            }
        }
    }

    public int GetPlayerDir()
    {
        if (PlayerManager.instance.player.transform.position.x < transform.position.x)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }

    public bool IsPlayerFaceToEnemy()
    {
        Player player = PlayerManager.instance.player;
        if(player.transform.position.x < transform.position.x)
        {
            return !player.isFacingLeft;
        }
        else
        {
            return player.isFacingLeft;
        }
    }

    public float CheckDistanceToPlayer()
    {
        Vector2 playerPos = PlayerManager.instance.player.transform.position;
        Vector2 enemyPos = transform.position;
        return Vector2.Distance(playerPos, enemyPos);
    }

    public override bool IsTouchWall()
    {
        return base.IsTouchWall() 
            || Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsPlatform); ;
    }
}
