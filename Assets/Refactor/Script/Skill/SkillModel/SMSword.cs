using EntitySystem;
using ObjectGenerateData;
using PlayerSystem;
using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace SkillSystem
{
    internal enum ESwordType
    {
        Regular,
        Spin,
        Bounce,
        Pierce
    }

    internal class SMSword : SMSkillModel
    {
        [Header("SwordSkill Info")]
        [SerializeField] protected Vector2 launchSpeed;
        [SerializeField] protected float swordGravity;
        [SerializeField] protected float returnSpeed;
        [SerializeField] protected float catchFeedback;
        [SerializeField] protected float swordCatchDistance;
        [SerializeField] protected float minDamageRate = 0.8f;
        [SerializeField] protected float enhenceDamageRate = 1.2f;
        [SerializeField] protected float forceSwordTakeBackRadius = 10f;

        [Header("Sword Time Stop")]
        [SerializeField] protected bool isUnlocked_swordTimeStop;
        [SerializeField] protected float swordHitFreezeEnemyTime;

        [Header("Sword Enhencemnet")]
        [SerializeField] protected bool isUnlocked_swordEnhencemnet;

        [Header("BounceSword Info")]
        [SerializeField] protected int bounceTime;
        [SerializeField] protected float bouncingRadius;
        [SerializeField] protected float bouncingSpeed;
        [SerializeField] protected float bouncingGravity;

        [Header("PierceSword Info")]
        [SerializeField] protected int pierceTime;
        [SerializeField] protected float pierceGravity;

        [Header("SpinSword Info")]
        [SerializeField] protected float spinDamageCooldown;
        [SerializeField] protected float spinDuration;
        [SerializeField] protected float spinGravity;

        [Header("Aimming Line")]
        [SerializeField] protected int dotNum;
        [SerializeField] protected float betweenDotSpace;

        [Header("Sword Cluster Info")]
        [SerializeField] protected bool isUnlocked_swordCluster;
        [SerializeField] protected float swordClusterCooldown;

        [Header("Test")]
        [SerializeField] protected ESwordType swordType = ESwordType.Regular;
        protected Vector3 aimDir;
        protected bool isAimming = false;
        protected DDamageData damage;
        protected ISkillObject swordObject;
        protected bool isTakingBack = false;

        protected void Update()
        {
            if(isAimming)
            {
                AimmingUpdate();
            }

            if (isTakingBack)
            {
                TakeBackUpdate();
            }

            if (swordObject != null)
            {
                ForceTakeBackCheck();
            }
        }

        protected void ForceTakeBackCheck()
        {
            Vector3 swordPosition = swordObject.GetTransform().position;
            Vector3 playerPosition = manager.CheckPlayerTransform().position;

            if (Vector3.Distance(swordPosition, playerPosition) > forceSwordTakeBackRadius)
            {
                TakeSword();
            }
        }

        protected void TakeBackUpdate()
        {
            Transform swordTransform = swordObject.GetTransform();
            swordTransform.position = Vector3.MoveTowards(swordTransform.position, manager.CheckPlayerTransform().position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(swordTransform.position, manager.CheckPlayerTransform().position) < swordCatchDistance)
            {
                swordObject.RecycleObject();
                isTakingBack = false;
                swordObject = null;
                manager.CatchSwordEnd();
            }
        }

        protected void AimmingUpdate()
        {
            Vector3 playerPosition = manager.CheckPlayerTransform().position;
            Vector3 mousePosition = manager.CheckMousePosition();
            aimDir = mousePosition - playerPosition;
            manager.AimmingUpdate(new DProjectileAimmingData(aimDir, launchSpeed, swordGravity));
        }

        public void AimmingBegin()
        {
            isAimming = true;
            manager.AimmingBegin();
        }

        public void AimmingEnd()
        {
            isAimming = false;
            manager.AimmingFinish();
        }

        public void SetSwordType(ESwordType _type)
        {
            Assert.IsTrue(swordType == ESwordType.Regular);
            switch(_type)
            {
                case ESwordType.Spin: SetSpinSword(); return;
                case ESwordType.Pierce: SetPierceSword(); return;
                case ESwordType.Bounce: SetBounceSword(); return;
            }
        }
        protected void SetSpinSword()
        {           
            swordType = ESwordType.Spin;
            swordGravity = spinGravity;
        }
        protected void SetPierceSword()
        {
            swordType = ESwordType.Pierce;
            swordGravity = pierceGravity;
        }
        protected void SetBounceSword()
        {
            swordType = ESwordType.Bounce;
            swordGravity = bouncingGravity;
        }

        public void ThrowSword()
        {
            switch(swordType)
            {
                case ESwordType.Regular: ThrowRegularSword(); return;
                case ESwordType.Spin:    ThrowSpinSword();    return;
                case ESwordType.Pierce:  ThrowPierceSword();  return;
                case ESwordType.Bounce:  ThrowBounceSword();  return;
            }
        }

        protected void ThrowRegularSword()
        {
            damage = manager.CheckPlayerPrimaryDamage().Clone();
            damage.physical *= minDamageRate;
            damage.magical *= minDamageRate;

            swordObject =
                manager.ThrowSword(
                    new DProjectileData(
                        new WReadOnlyDamageData(damage),
                        EEntityType.Enemy, aimDir.normalized * launchSpeed,
                        swordGravity
                        )
                    );
        }
        protected void ThrowSpinSword()
        {
            damage = manager.CheckPlayerPrimaryDamage().Clone();
            damage.physical *= minDamageRate;
            damage.magical *= minDamageRate;

            swordObject =
                manager.ThrowSpinSword(
                    new DSpinSwordData(
                        new WReadOnlyDamageData(damage),
                        aimDir.normalized * launchSpeed, swordGravity,
                        spinDuration,
                        spinDamageCooldown
                        )
                    );
        }
        protected void ThrowPierceSword()
        {
            damage = manager.CheckPlayerPrimaryDamage().Clone();
            damage.physical *= minDamageRate;
            damage.magical *= minDamageRate;

            swordObject =
                manager.ThrowPierceSword(
                    new DProjectileData(
                        new WReadOnlyDamageData(damage),
                        EEntityType.Enemy, aimDir.normalized * launchSpeed,
                        swordGravity
                        )
                    );
        }
        protected void ThrowBounceSword()
        {
            damage = manager.CheckPlayerPrimaryDamage().Clone();
            damage.physical *= minDamageRate;
            damage.magical *= minDamageRate;

            swordObject =
                manager.ThrowBounceSword(
                    new DBounceSwordData(
                        new WReadOnlyDamageData(damage),
                        EEntityType.Enemy, aimDir.normalized * launchSpeed, swordGravity,
                        bounceTime, bouncingSpeed, bouncingRadius
                        )
                    );
        }

        public void TakeSword()
        {
            isTakingBack = true;
            swordObject.TakeBack();
            manager.CatchSwordBegin();
        }

        public bool IsSwordThrown()
        {
            return swordObject == null;
        }
    }
}

