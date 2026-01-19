using EntitySystem.EntityActor;
using EntitySystem.EntityActor.PlayerActor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Windows;

namespace EntitySystem
{
    namespace EntityComponent
    {
        namespace BattleComponent
        {
            internal class CPlayerBattle : CEntityBattle
            {
                protected APlayer player;

                [Header("Fight Info")]
                [SerializeField] public float comboWindow;
                [SerializeField] public int comboAmount = 3;
                [SerializeField] public LayerMask whatIsEnemy;


                protected int comboCounter = 0;
                protected float lastAttackTime;

                protected override void Awake()
                {
                    base.Awake();
                    Assert.IsTrue(entity is APlayer, "CPlayerBattle组件必须附加于APlayer实体");
                    player = entity as APlayer;
                    player.AttackRaw += Attack;
                    player.AttackDamageTrigger += DamageTrigger;
                }

                protected void Attack()
                {
                    if (Time.time - lastAttackTime > comboWindow)
                    {
                        comboCounter = 0;
                    }
                    else
                    {
                        ++comboCounter;
                        comboCounter %= comboAmount;
                    }

                    lastAttackTime = Time.time;
                    player.InvokeAction(player.Attack, comboCounter);
                }

                protected void DamageTrigger()
                {
                    WReadOnlyDamageData damageData = player.InvokeFunc(player.GetPrimaryAttackDamage);
                    Collider2D[] allHitEnemy = Physics2D.OverlapCircleAll(attackValidCheck.position, attackValidCheckRadius, whatIsEnemy);
                    foreach (var hit in allHitEnemy)
                    {
                        IPlayerEnemy enemy = hit.GetComponent<IPlayerEnemy>();
                        if (enemy != null)
                        {
                            enemy.TakeDamage(player.InvokeFunc(player.GetPrimaryAttackDamage));
                        }
                    }
                }
            }
        }
    }
}
