using EntitySystem.EntityActor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AnimationAndFx
{
    public class MEntityAnimation : MonoBehaviour, IEntityAnimation
    {
        protected string currentAnimName = "";
        protected Animator anim;
        protected bool isUpdateYVelocity;

        protected virtual void Awake()
        {
            anim = GetComponent<Animator>();
            Assert.IsNotNull(anim, "实体动画系统需要管理一个Animator组件");
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
    }
}

