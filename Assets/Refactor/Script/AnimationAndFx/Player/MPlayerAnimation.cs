using EntitySystem.EntityActor;
using EntitySystem.EntityActor.PlayerActor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AnimationAndFx
{
    public class MPlayerAnimation : MEntityAnimation, IPlayerAnimation
    {
        protected IAnimPlayer player;

        [SerializeField] protected string yVelocityName = "yVelocity";
        [SerializeField] protected string comboCounterName = "ComboCounter";

        protected override void Awake()
        {
            base.Awake();
            player = GetComponentInParent<IAnimPlayer>();
            Assert.IsNotNull(player, "MPlayerAnimation需要挂载在一个Player上");
        }

        public void Attack(int _count)
        {
            anim.SetInteger(comboCounterName, _count);
        }

        public void UpdateYVelocity(float _yVelocity)
        {
            anim.SetFloat(yVelocityName, _yVelocity);
        }

        public void OnAttackFinish()
        {
            player.AttackFinish();
        }

        public void OnAttackDamageTrigger()
        {
            player.AttackDamageTrigger();
        }
    }
}
