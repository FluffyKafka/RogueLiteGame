using EntityBehaviour;
using StatsData;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

namespace EnemyBehaviour
{
    internal class CEnemyBattle : CEntityBattle
    {
        protected MEnemyBehaviour enemy;

        #region PlayerDetect
        [Header("EnemyBase Player Detect")]
        [SerializeField] public LayerMask whatIsGround;
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
        [SerializeField] public float battleDuration;
        [SerializeField] public float battleIdleRadius;
        [SerializeField] public float stunDuration;
        #endregion

        protected bool canBeStunned = false;
        protected bool isAttackCooldown = false;

        protected float battleTimer = -1;

        protected override void Awake()
        {
            base.Awake();
            Assert.IsTrue(entity is MEnemyBehaviour, "此为Enemy组件");
            enemy = entity as MEnemyBehaviour;

            enemy.IsDetectPlayer += IsDetectPlayer;
            enemy.UpdateBattle += UpdateBattle;
            enemy.StunCheck += StunCheck;
            enemy.CheckBattleMoveDir += CheckMoveDir;
            enemy.AttackCheck += AttackCheck;

            enemy.OpenStun += OpenStun;
            enemy.AttackDamageTrigger += DamageTrigger;
        }

        protected override void Update()
        {
            base.Update();
            if (battleTimer >= 0)
            {
                battleTimer -= Time.deltaTime;
                float eclipse = 0.01f;
                if (battleTimer < eclipse)
                {
                    enemy.InvokeAction(enemy.StopBattle);
                    battleTimer = -1;
                }
            }
        }

        protected virtual bool IsDetectPlayer()
        {
            return IsPlayerDetectedInRadius() || IsDetectPlayerFront();
        }

        protected virtual RaycastHit2D IsDetectPlayerFront()
        {
            int facingDir = enemy.InvokeFunc(enemy.CheckFacingDir);
            RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDistance, whatIsPlayer);

            if (hit)
            {
                bool isPlayer = enemy.InvokeFunc(enemy.IsPlayer, hit.transform.gameObject);
                bool isPlayerAlive = enemy.InvokeFunc(enemy.IsThisPlayerAlive, hit.transform.gameObject);
                bool canSeePlayer = CanSeePlayer();
                if (isPlayer && isPlayerAlive && canSeePlayer)
                {
                    return hit;
                }
                else
                {
                    return default;
                }

            }
            return hit;
        }

        protected virtual bool IsPlayerDetectedInRadius()
        {
            bool isDetect =
                enemy.InvokeFunc(enemy.IsPlayerAlive) &&
                Vector2.Distance(enemy.InvokeFunc(enemy.CheckPlayerPosition), transform.position) < playerDetectRadius && CanSeePlayer();
            return isDetect;
        }

        protected bool CanSeePlayer()
        {
            return !Physics2D.Linecast(playerCheck.position, enemy.InvokeFunc(enemy.CheckPlayerPosition), whatIsGround);
        }

        protected void UpdateBattle()
        {
            if (IsDetectPlayer())
            {
                battleTimer = battleDuration;
            }
        }

        protected int CheckMoveDir()
        {
            if (math.abs(enemy.InvokeFunc(enemy.CheckPlayerPosition).x - enemy.transform.position.x) < battleIdleRadius)
            {
                return 0;
            }

            if (enemy.InvokeFunc(enemy.CheckPlayerPosition).x < enemy.transform.position.x)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        protected virtual void AttackCheck()
        {
            if (!isAttackCooldown)
            {
                RaycastHit2D hit = IsDetectPlayerFront();
                if (hit && hit.distance <= toAttackRadius)
                {
                    enemy.InvokeAction(enemy.Attack);
                    StartCoroutine(AttackCoolDownHelper());
                }
            }
        }
        protected IEnumerator AttackCoolDownHelper()
        {
            isAttackCooldown = true;
            float currentAttackCooldown = UnityEngine.Random.Range(minAttackCooldown, maxAttackCooldown);
            yield return new WaitForSeconds(currentAttackCooldown);
            isAttackCooldown = false;
        }

        protected void OpenStun(bool _isOpen)
        {
            canBeStunned = _isOpen;
        }

        protected void StunCheck()
        {
            if (canBeStunned)
            {
                enemy.InvokeAction(enemy.BeStunned);
            }
        }
        protected IEnumerator StunFinishHelper()
        {
            yield return new WaitForSeconds(stunDuration);
            enemy.InvokeAction(enemy.StunFinish);
        }

        protected virtual void DamageTrigger()
        {
            WReadOnlyDamageData damageData = enemy.InvokeFunc(enemy.GetPrimaryAttackDamage);
            Collider2D[] allHitEnemy = Physics2D.OverlapCircleAll(attackValidCheck.position, attackValidCheckRadius, whatIsPlayer);
            foreach (var hit in allHitEnemy)
            {
                if (enemy.InvokeFunc(enemy.IsPlayer, hit.gameObject) && enemy.InvokeFunc(enemy.IsThisPlayerAlive, hit.gameObject))
                {
                    WReadOnlyDamageData damage = enemy.InvokeFunc(enemy.GetPrimaryAttackDamage);                    
                    WReadOnlyDamageData realDamage = enemy.InvokeFunc(enemy.DamageToPlayer, hit.gameObject, damage);
                }
            }
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + playerCheckDistance, playerCheck.position.y));
            Gizmos.DrawWireSphere(playerCheck.position, playerDetectRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, toAttackRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, battleIdleRadius);
        }
    }
}

