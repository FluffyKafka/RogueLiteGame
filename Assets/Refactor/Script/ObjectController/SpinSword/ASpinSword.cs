using EnemySystem;
using EntitySystem;
using ObjectGenerateData;
using SkillSystem;
using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class ASpinSword : AObjectController
    {
        protected WReadOnlyDamageData damage;
        protected ISwordObjectModel swordModel;
        protected bool isHit = false;
        protected bool canDamage = false;
        protected float spinSpeed;
        protected float spinDistance;
        protected float damageCooldown;
        protected float spinDuration;

        public void Setup(FCSpinSwordFactory _factory, DSpinSwordData _data)
        {
            factory = _factory;

            damage = _data.damage;
            isHit = false;
            canDamage = false;
            damageCooldown = _data.damageCooldown;
            spinDuration = _data.spinDuration;
            swordModel = _data.manager.GetComponent<ISwordObjectModel>();
            GetComponent<Rigidbody2D>().gravityScale = _data.gravity;
            InvokeAction(SwitchTargetTo, EEntityType.Enemy);
            InvokeAction(Project, _data.velocity);
            InvokeAction(SetLookAtMovement, true);

            HitEnemyNotice += DamageToEnemy;
            HitGroundNotice += HitGround;
        }

        protected void Update()
        {
            if (isHit && canDamage)
            {
                InvokeAction(EffectAreaDamageTo, damage, EEntityType.Enemy);
                StartCoroutine(DamageCooldown());
            }
        }
        protected IEnumerator DamageCooldown()
        {
            canDamage = false;
            yield return new WaitForSeconds(damageCooldown);
            canDamage = true;
        }

        protected IEnumerator SelfRecycleAfter(float _time)
        {
            yield return new WaitForSeconds(_time);
            SelfRecycle();
        }

        protected void DamageToEnemy(IObjectEnemy _enemy)
        {
            _enemy.TakeObjectDamage(damage);
            anim.ShowHitFx();
            StartCoroutine(Spin());
            canDamage = true;
        }
        protected IEnumerator Spin()
        {
            isHit = true;
            Rigidbody2D rg = GetComponent<Rigidbody2D>();
            float gravity = rg.gravityScale;
            rg.gravityScale = 0;
            rg.isKinematic = true;
            rg.velocity = Vector2.zero;
            yield return new WaitForSeconds(spinDuration);
            rg.isKinematic = false;
            rg.gravityScale = gravity;
            isHit = false;
        }


        protected void HitGround(Transform _ground)
        {
            swordModel.HitGround();
            InvokeAction(StuckInto, _ground);
            anim.ToEffect();
            anim.ShowHitFx();
            InvokeAction(SetLookAtMovement, false);
        }

        public override void TakeBack()
        {
            base.TakeBack();
            InvokeAction(StuckInto, null);
        }

        public override void Clear()
        {
            base.Clear();
            HitEnemyNotice -= DamageToEnemy;
            HitGroundNotice -= HitGround;
            StopAllCoroutines();
            isHit = false;
            canDamage = false;
        }
    }
}

