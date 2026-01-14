using EntitySystem.EntityActor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AnimationAndFx
{
    public class MEntityAnimation : MonoBehaviour, IEntityAnimation
    {
        protected IAnimationController controller;
        public string currentAnimName = "";
        protected Animator anim;

        protected void Awake()
        {
            anim = GetComponent<Animator>();
            Assert.IsNotNull(anim, "实体动画系统需要管理一个Animator组件");
        }

        public void ChangeAnimationNotice()
        {
            controller = GetComponentInParent<IAnimationController>();
            string targetAnimName = controller.CheckStateAnimationName();
            if (currentAnimName == targetAnimName)
            {
                return;
            }

            if(currentAnimName != "")
            {
                anim.SetBool(currentAnimName, false);
            }

            
            anim.SetBool(targetAnimName, true);
            currentAnimName = targetAnimName;
        }
    }
}

