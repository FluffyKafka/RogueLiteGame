using EntitySystem.EntityActor.EnemyActor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AnimationAndFx
{
    public class MEnemyAnimation : MEntityAnimation
    {
        protected IAnimEnemy enemy;

        protected override void Awake()
        {
            base.Awake();
            enemy = GetComponentInParent<IAnimEnemy>();
            Assert.IsNotNull(enemy, "MEnemyAnimation需要挂载在一个Enemy上");
        }
        public void OnAttackFinish()
        {
            enemy.AttackFinish();
        }

        public void OnAttackDamageTrigger()
        {
            enemy.AttackDamageTrigger();
        }

        public void OnStunOpen()
        {
            enemy.OpenStun(true);
        }

        public void OnStunClose()
        {
            enemy.OpenStun(false);
        }
    }
}
