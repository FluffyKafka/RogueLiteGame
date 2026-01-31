using EntityBehaviour;
using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace PlayerBebaviour
{
    internal class CPlayerBattle : CEntityBattle
    {
        protected MPlayerBeviour player;

        [Header("Fight Info")]
        [SerializeField] public float comboWindow;
        [SerializeField] public int comboAmount = 3;
        [SerializeField] public LayerMask whatIsEnemy;


        protected int comboCounter = 0;
        protected float lastAttackTime;

        protected override void Awake()
        {
            base.Awake();
            Assert.IsTrue(entity is MPlayerBeviour, "CPlayerBattle组件必须附加于APlayer实体");
            player = entity as MPlayerBeviour;
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
                if (player.InvokeFunc(player.IsEnemy, hit.gameObject) && player.InvokeFunc(player.IsEnemyAlive, hit.gameObject))
                {
                    WReadOnlyDamageData damage = player.InvokeFunc(player.GetPrimaryAttackDamage);
                    WReadOnlyDamageData realDamage = player.InvokeFunc(player.DamageTo, hit.gameObject, damage);
                }
            }
        }
    }
}
