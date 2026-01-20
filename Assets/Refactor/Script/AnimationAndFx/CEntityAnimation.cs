using EntitySystem.EntityActor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AnimationAndFx
{
    internal class CEntityAnimation : CEntityAnimFxComponentBase
    {        
        protected string currentAnimName = "";
        protected Animator anim;

        protected override void Awake()
        {
            anim = GetComponent<Animator>();
            Assert.IsNotNull(anim, "动画组件需要管理一个Animator组件");

            animFxSystem.SlowBy += SlowBy;
            animFxSystem.RecoverSpeed += RecoverSpeed;
        }

        protected void ChangeAnimationTo(string _stateAnimName)
        {
            if (currentAnimName == _stateAnimName)
            {
                return;
            }

            if(currentAnimName != "")
            {
                anim.SetBool(currentAnimName, false);
            }

            
            anim.SetBool(_stateAnimName, true);
            currentAnimName = _stateAnimName;
        }

        protected void SlowBy(float _rate)
        {
            anim.speed *= (1 - _rate);
        }

        protected void RecoverSpeed()
        {
            anim.speed = 1;
        }
    }
}

