using EntitySystem.EntityActor;
using EntitySystem.EntityActor.EnemyActor;
using EntitySystem.EntityActor.PlayerActor;
using StatsData;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

namespace EntitySystem
{
    namespace EntityComponent
    {
        namespace BattleComponent
        {
            internal class CEnemyBattle : CEntityBattle
            {
                protected AEnemy enemy;

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
                    Assert.IsTrue(entity is AEnemy, "此为Enemy组件");
                    enemy = entity as AEnemy;

                    enemy.IsDetectPlayer += IsDetectPlayer;
                    enemy.UpdateBattle += UpdateBattle;
                    enemy.OpenStun += OpenStun;
                    enemy.StunCheck += StunCheck;
                    enemy.CheckBattleMoveDir += CheckMoveDir;
                    enemy.AttackCheck += AttackCheck;
                    enemy.AttackDamageTrigger += DamageTrigger;
                }

                protected override void Update()
                {
                    base.Update();
                    if(battleTimer >= 0)
                    {
                        battleTimer -= Time.deltaTime;
                        float eclipse = 0.01f;
                        if(battleTimer < eclipse)
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
                    
                    if(hit)
                    {
                        IEnemyPlayer player = hit.collider.GetComponent<IEnemyPlayer>();
                        if(player != null && !hit.collider.GetComponent<IEnemyPlayer>().IsDead() && CanSeePlayer())
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
                        !enemy.player.IsDead() &&
                        Vector2.Distance(enemy.player.CheckPosition(), transform.position) < playerDetectRadius && CanSeePlayer();
                    return isDetect;
                }

                protected bool CanSeePlayer()
                {
                    return !Physics2D.Linecast(playerCheck.position, enemy.player.CheckPosition(), whatIsGround);
                }

                protected void UpdateBattle()
                {
                    if(IsDetectPlayer())
                    {
                        battleTimer = battleDuration;
                    }
                }

                protected int CheckMoveDir()
                {
                    if(math.abs(enemy.player.CheckPosition().x - enemy.transform.position.x) < battleIdleRadius)
                    {
                        return 0;
                    }

                    if (enemy.player.CheckPosition().x < enemy.transform.position.x)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }

                protected void AttackCheck()
                {                   
                    if(!isAttackCooldown)
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
                    if(canBeStunned)
                    {
                        enemy.InvokeAction(enemy.BeStunned);
                    }
                }
                protected IEnumerator StunFinishHelper()
                {
                    yield return new WaitForSeconds(stunDuration);
                    enemy.InvokeAction(enemy.StunFinish);
                }

                protected void DamageTrigger()
                {
                    WReadOnlyDamageData damageData = enemy.InvokeFunc(enemy.GetPrimaryAttackDamage);
                    Collider2D[] allHitEnemy = Physics2D.OverlapCircleAll(attackValidCheck.position, attackValidCheckRadius, whatIsPlayer);
                    foreach (var hit in allHitEnemy)
                    {
                        IEnemyPlayer player = hit.GetComponent<IEnemyPlayer>();
                        if (player != null)
                        {
                            WReadOnlyDamageData realDamage = player.TakeDamage(enemy.InvokeFunc(enemy.GetPrimaryAttackDamage));
                        }
                    }
                }
            }
        }
    }
}

