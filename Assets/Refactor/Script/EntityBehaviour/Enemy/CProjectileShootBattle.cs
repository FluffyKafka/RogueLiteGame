
using EntitySystem;
using ObjectGenerateData;
using StatsData;
using UnityEngine;

namespace EnemyBehaviour
{
    internal enum ProjectileBulletType
    {
        Arrow
    }
    internal class CProjectileShootBattle : CEnemyBattle
    {
        [SerializeField] protected float arrowSpeedReference;
        [SerializeField] protected float speedMapK;
        [SerializeField] protected Transform arrowShootTransform;
        [SerializeField] protected ProjectileBulletType bulletType;

        protected override void Awake()
        {
            base.Awake();
        }
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
                case ProjectileBulletType.Arrow:
                    enemy.InvokeAction(enemy.GenerateArrowAt, new DProjectileData(damageData, EEntityType.Player, CulculateArrowVelocity()), arrowShootTransform.position);break;
            }
            
        }
        protected Vector2 CulculateArrowVelocity()
        {
            Vector3 playerPosition = enemy.InvokeFunc(enemy.CheckPlayerPosition);
            Vector3 playerVelocity = enemy.InvokeFunc(enemy.CheckPlayerVelocity);
            float playerGravityScale = enemy.InvokeFunc(enemy.CheckPlayerGravityScale);
            float arrowGravityScale = enemy.InvokeFunc(enemy.CheckArrowGravity);

            float distanceToPlayer = Vector2.Distance(playerPosition, transform.position);
            float timeToHit = distanceToPlayer / arrowSpeedReference;

            Vector2 sourcePosition = transform.position;

            float affectFactor = 1 - 1 / Mathf.Pow(1 + arrowSpeedReference, speedMapK);
            Vector2 targetPositionAfterTimeToHit = playerPosition + playerVelocity * timeToHit * affectFactor;
            if (playerVelocity.y != 0)
            {
                targetPositionAfterTimeToHit += 0.5f * Physics2D.gravity * playerGravityScale * timeToHit * timeToHit * affectFactor;
            }

            Vector2 arrowVelocity = (
                        targetPositionAfterTimeToHit -
                        sourcePosition -
                        0.5f * (Physics2D.gravity * arrowGravityScale) * (timeToHit * timeToHit)
                    ) / timeToHit;

            return arrowVelocity;
        }
    }
}

