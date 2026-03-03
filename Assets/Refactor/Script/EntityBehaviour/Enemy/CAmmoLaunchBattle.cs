using EntitySystem;
using ObjectGenerateData;
using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviour
{
    internal enum EAmmoBulletType
    {
        SkullAmmo
    }

    internal class CAmmoLaunchBattle : CEnemyBattle
    {
        [SerializeField] Transform ammoGenerateTransform;
        [SerializeField] EAmmoBulletType bulletType;

        protected override void AttackCheck()
        {
            if (!isAttackCooldown)
            {
                bool canAttack =
                    enemy.InvokeFunc(enemy.IsPlayerAlive) &&
                    Vector2.Distance(enemy.InvokeFunc(enemy.CheckPlayerPosition), transform.position) < toAttackRadius &&
                    CanSeePlayer();
                if (canAttack)
                {
                    enemy.InvokeAction(enemy.Attack);
                    StartCoroutine(AttackCoolDownHelper());
                }
            }
        }

        protected override void DamageTrigger()
        {
            WReadOnlyDamageData damageData = enemy.InvokeFunc(enemy.GetPrimaryAttackDamage);
            switch(bulletType)
            {
                case EAmmoBulletType.SkullAmmo:
                enemy.InvokeAction(
                    enemy.GenerateSkullAmmo, 
                    new DAmmoData(damageData, EEntityType.Player, enemy.InvokeFunc(enemy.CheckPlayerTransform), null), 
                    ammoGenerateTransform.position
                    );
                break;
            }
        }
    }
}

