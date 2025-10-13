using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public interface IPlayerEnterable
{
    public void Enter(Player _player);
}

public interface IPlayerCommunicable
{
    public void Talk(Player _player);
}
public class Player : Entity
{
    public bool isBusy { get; private set; }
    public bool isDashing = false;
    public bool canInput = true;

    #region Move
    [Header("Move Info")]
    [SerializeField] public float moveSpeed;
    private float defaultMoveSpeed;
    [SerializeField] public float jumpSpeed;
    private float defaultJumpSpeed;
    [SerializeField] public float moveInAirSpeed;
    [SerializeField] public float wallSlideSpeed;
    [SerializeField] public float wallSlideUpAdjustSpeed;
    [SerializeField] public float wallSlideDownAdjustSpeed;
    [SerializeField] public float wallJumpHorizontalSpeed;
    #endregion

    #region Dash
    [Header("Dash Info")]
    [SerializeField] public float dashDuration;
    [SerializeField] public float dashSpeed;
    private float defalutDashSpeed;
    public float dashDir { get; private set; }
    #endregion

    #region Fight
    [Header("Fight Info")]
    [SerializeField] public float comboWindow;
    [SerializeField] public float movableDurationInAttacking;
    [SerializeField] public float unmovableDurationAfterAttack;
    [SerializeField] public float[] attackMovement;
    [SerializeField] public float counterAttackDuration = 0.2f;
    [SerializeField] public float enemyCanBeStunnedCheckRadius;
    [SerializeField] public float unmovebleDurationAfterThrowSword;
    [SerializeField] public float unmovebleDurationAfterCatchSword;
    #endregion

    #region States
    public PlayerStateMachine stateMachine { get; private set; } 
    public PlayerState idleState { get; private set; }
    public PlayerState moveState { get; private set; }
    public PlayerState jumpState { get; private set; }
    public PlayerState fallState { get; private set; }
    public PlayerState dashState { get; private set; }
    public PlayerState wallSlideState { get; private set; }
    public PlayerState wallJumpState { get; private set; }
    public PlayerState primaryAttackState { get; private set; }
    public PlayerState counterAttackState { get; private set; }
    public PlayerState successfulCounterAttackState { get; private set; }
    public PlayerState aimSwordState { get; private set; }
    public PlayerState catchSwordState { get; private set; }
    public PlayerState blackHoleState { get; private set; }
    public PlayerState deadState { get; private set; }
    #endregion

    #region Component
    public SkillManager skill { get; private set; }
    public GameObject swordThrown;
    public PlayerEnemyCheck enemyCheck { get; private set; }
    #endregion

    [Header("Health Bar")]
    [SerializeField] public GameObject healthBar;
    public bool isHealthBarActive;

    [Header("Enter Check Info")]
    [SerializeField] private float enterCheckRadius = 2f;
    [SerializeField] private LayerMask whatIsDoor;

    [Header("Craft BlackSmith Detect")]
    [SerializeField] private float craftDetectRadius;
    override protected void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(stateMachine, this, "Idle");
        moveState = new PlayerMoveState(stateMachine, this, "Move");
        jumpState = new PlayerJumpState(stateMachine, this, "Air");
        fallState = new PlayerFallState(stateMachine, this, "Air");
        dashState = new PlayerDashState(stateMachine, this, "Dash");
        wallSlideState = new PlayerWallSlideState(stateMachine, this, "WallSlide");
        wallJumpState = new PlayerWallJumpState(stateMachine, this, "Air");
        primaryAttackState = new PlayerPrimaryAttackState(stateMachine, this, "Attack");
        counterAttackState = new PlayerCounterAttackState(stateMachine, this, "CounterAttack");
        successfulCounterAttackState = new PlayerSuccessfulCounterAttackState(stateMachine, this, "SuccessfulCounterAttack");
        aimSwordState = new PlayerAimSwordState(stateMachine, this, "AimSword");
        catchSwordState = new PlayerCatchSwordState(stateMachine, this, "CatchSword");
        blackHoleState = new PlayerBlackHoleState(stateMachine, this, "Air");
        deadState = new PlayerDeadState(stateMachine, this, "Dead");

        isHealthBarActive = healthBar.activeSelf;

        enemyCheck = GetComponent<PlayerEnemyCheck>();
    }

    override protected void Start()
    {
        base.Start();

        skill = SkillManager.intance;

        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpSpeed = jumpSpeed;
        defalutDashSpeed = dashSpeed;
    }

    override protected void Update()
    {
        if(Time.timeScale == 0)
        {
            return;
        }

        base.Update();
        stateMachine.currentState.Update();
        if (!IsInStateNotAllowDash())
        {
            DashInputCheck();
        }
        if(!IsInStateNotAllowPutCrystal())
        {
            CrystalInputCheck();
        }
        FlaskUseCheck();
        EnterCheck();
        CommunicateCheck();
    }

    private void EnterCheck()
    {
        if(CheckInput_KeyDown(KeyCode.W))
        {
            Collider2D collider = Physics2D.OverlapCircle(transform.position, enterCheckRadius, whatIsDoor);
            if(collider != null)
            {
                if (collider.GetComponent<IPlayerEnterable>() != null)
                {
                    collider.GetComponent<IPlayerEnterable>().Enter(this);
                }
            }
        }
    }

    private void CommunicateCheck()
    {
        if (CheckInput_KeyDown(KeyCode.G))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enterCheckRadius);
            foreach(Collider2D collider in colliders)
            {
                if (collider != null)
                {
                    if (collider.GetComponent<IPlayerCommunicable>() != null)
                    {
                        collider.GetComponent<IPlayerCommunicable>().Talk(this);
                    }
                }
            }
        }
    }

    public void SetHealthBarActiveChange()
    {
        isHealthBarActive = !isHealthBarActive;
        healthBar.SetActive(isHealthBarActive);
    }

    public override void MakeTransprent(bool _isTransprent)
    {
        base.MakeTransprent(_isTransprent);
        if (_isTransprent)
        {
            healthBar.SetActive(false);
        }
        else
        {
            healthBar.SetActive(isHealthBarActive);
        }
    }

    public override CharacterStats GetStats()
    {
        return cs;
    }


    private void CrystalInputCheck()
    {
        if (CheckInput_KeyDown(KeyCode.F))
        {
            SkillManager.intance.crystal.TryUseSkill();
        }
    }
    private bool IsInStateNotAllowPutCrystal()
    {
        return stateMachine.currentState == blackHoleState;
    }

    private void DashInputCheck()
    {
        if(!skill.dash.isUnlocked_dash)
        {
            return;
        }

        if(CheckInput_KeyDown(KeyCode.LeftShift) && !IsTouchWall() && skill.dash.TryUseSkill() && !isKnocked)
        {
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
            {
                dashDir = facingDir;
            }
            stateMachine.ChangeState(dashState);
        }
    }
    private bool IsInStateNotAllowDash()
    {
        return !isKnocked && stateMachine.currentState == blackHoleState;
    }

    private void FlaskUseCheck()
    {
        if(CheckInput_KeyDown(KeyCode.R))
        {
            Inventory.instance.TryUseFlask();
        }
    }

    public IEnumerator BusyFor(float _second)
    {
        isBusy = true;
        yield return new WaitForSeconds(_second);
        isBusy = false;
    }

    public void AnimFinishTrigger()
    {
        stateMachine.currentState.AnimFinishTrigger();
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, enemyCanBeStunnedCheckRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, craftDetectRadius);
    }

    public bool IsBlackSmithAround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, craftDetectRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<BlackSmith>() != null)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsEnemyAroundCanBeStunned()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyCanBeStunnedCheckRadius);
        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                if(hit.GetComponent<Enemy>().CanBeStunned())
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void AssignThrownSword(GameObject _newSword)
    {
        swordThrown = _newSword;
    }

    public void ClearTheThrownSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(swordThrown);
    }

    public bool IsSwordThrown()
    {
        return swordThrown != null;
    }

    public override void Die()
    {
        if(!isDead)
        {
            stateMachine.ChangeState(deadState);
            base.Die();
        }
    }

    public override void SlowEntityBy(float _slowPercent, float _slowDuration)
    {
        base.SlowEntityBy(_slowPercent, _slowDuration);

        moveSpeed *= (1 - _slowPercent);
        jumpSpeed *= (1 - _slowPercent);
        dashSpeed *= (1 - _slowPercent);

        Invoke("ResetDefaultSpeed", _slowDuration);
    }

    protected override void ResetDefaultSpeed()
    {
        base.ResetDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
        jumpSpeed = defaultJumpSpeed;
        dashSpeed = defalutDashSpeed;
    }

    public virtual void DoDamageTo_PrimaryAttack(Entity _target)
    {
        float damage = cs.DoDamageTo_PrimaryAttack(_target);
        EffectExcuteData data = new EffectExcuteData(EffectExcuteTime.PrimaryAttack, _target, damage);
        Inventory.instance.TryGetEquipmentByType(EquipmentType.Weapon)?.TryExcuteItemEffect(data);
    }

    public virtual void DoDamageTo_Clone(Entity _target, float _damageRate, Transform _swordTransform)
    {
        float damage = (cs as PlayerStats).DoDamageTo_Clone(_target, _damageRate, _swordTransform);
        EffectExcuteData data = new EffectExcuteData(EffectExcuteTime.Clone, _target, damage);
        Inventory.instance.TryGetEquipmentByType(EquipmentType.Amulet)?.TryExcuteItemEffect(data);
    }

    public virtual void DoDamageTo_Crystal(Entity _target, Transform _crystalTransform)
    {
        float damage = (cs as PlayerStats).DoDamageTo_Crystal(_target, _crystalTransform);
        EffectExcuteData data = new EffectExcuteData(EffectExcuteTime.Crystal, _target, damage);
        Inventory.instance.TryGetEquipmentByType(EquipmentType.Amulet)?.TryExcuteItemEffect(data);
    }

    public virtual void DoDamageTo_Sword(Entity _target, float _damageRate, Transform _swordTransform)
    {
        float damage = (cs as PlayerStats).DoDamageTo_Sword(_target, _damageRate, _swordTransform);
        EffectExcuteData data = new EffectExcuteData(EffectExcuteTime.Sword, _target, damage);
        Inventory.instance.TryGetEquipmentByType(EquipmentType.Amulet)?.TryExcuteItemEffect(data);
    }

    public virtual void DoDamageTo_CounterAttack(Entity _target)
    {
        float damage = (cs as PlayerStats).DoDamageTo_CounterAttack(_target);
        EffectExcuteData data = new EffectExcuteData(EffectExcuteTime.CounterAttack, _target, damage);
        Inventory.instance.TryGetEquipmentByType(EquipmentType.Amulet)?.TryExcuteItemEffect(data);
    }

    public void SetCanInput(bool _canInput) 
    {
        if(_canInput)
        {
            Invoke("SetCanInput_true", Time.deltaTime * 2);
        }
        else
        {
            CancelInvoke();
            canInput = false;
        }
    }
    private void SetCanInput_true()
    {
        canInput = true;
    }

    public bool CheckInput_KeyDown(KeyCode _keyCode)
    {
        if(canInput)
        {
            return Input.GetKeyDown(_keyCode);
        }
        return false;
    }
}