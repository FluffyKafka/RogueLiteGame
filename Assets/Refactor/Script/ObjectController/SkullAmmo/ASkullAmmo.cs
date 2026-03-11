using EnemySystem;
using EntitySystem;
using ObjectGenerateData;
using PlayerSystem;
using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace ObjectController
{
    internal class ASkullAmmo : AObjectController
    {
        [SerializeField] protected float reflectPhysicalDamageMultiplier;
        [SerializeField] protected float lifeTime = 10f;
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float reflectMoveSpeed;

        protected DDamageData damage;
        protected EEntityType target;
        protected IObjectEntity origin;

        public void Setup(FCSkullAmmoFactory _factory, DAmmoData _data)
        {
            factory = _factory;

            damage = _data.damage.Clone();
            InvokeAction(Launch);
            InvokeAction(SwitchTargetTo, _data.targetType);
            target = _data.targetType;
            anim.ShowTrail(true);

            PlayerReflect += BeReflect;
            HitPlayer += HitEntityHandle;
            HitEnemyNotice += HitEntityHandle;
            HitGroundNotice += HitGroundHandle;
            InvokeAction(SetLookAtMovement, true);
            InvokeAction(SetMoveToTargetNotice, _data.target, moveSpeed);
            DamageFinishNotice += SelfRecycle;
            origin = _data.originEntity;

            StartCoroutine(SelfRecycleAfter(lifeTime));
        }

        protected IEnumerator SelfRecycleAfter(float _time)
        {
            yield return new WaitForSeconds(_time);
            SelfRecycle();
        }

        public void BeReflect(IObjectPlayer _player)
        {
            if (target == EEntityType.Player)
            {
                damage.physical *= reflectPhysicalDamageMultiplier;
                target = EEntityType.Enemy;
                InvokeAction(SwitchTargetTo, EEntityType.Enemy);
                InvokeAction(SetMoveToTargetNotice, origin.CheckTransform(), reflectMoveSpeed);
            }
        }

        protected void Expolde()
        {
            anim.ShowTrail(false);
            anim.ToEffect();
            InvokeAction(SetLookAtMovement, false);
            InvokeAction(SetMoveToTargetNotice, null, 0);
            DamageTriggerNotice += ExplodeDamage;
            origin.ObjectFinish(transform);
        }
        protected void ExplodeDamage()
        {
            DamageTriggerNotice -= ExplodeDamage;           
            InvokeAction(EffectAreaDamageTo, new WReadOnlyDamageData(damage), target);
        }

        protected void HitEntityHandle(IObjectEntity _entity)
        {
            Expolde();
        }

        protected void HitGroundHandle(Transform _ground)
        {
            Expolde();
        }

        public override void Clear()
        {
            base.Clear();
            damage = null;
            origin = null;
            FadeFinish -= SelfRecycle;
            PlayerReflect -= BeReflect;
            HitPlayer -= HitEntityHandle;
            HitEnemyNotice -= HitEntityHandle;
            HitGroundNotice -= HitGroundHandle;
            StopAllCoroutines();
        }
    }
}

