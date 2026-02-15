using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationAndFx
{
    internal class CAnimatorDisplay : CObjectAnimationComponentBase
    {
        [SerializeField] protected string idleAnimName = "Idle";
        [SerializeField] protected string effectAnimName = "Effect";

        [Header("Test")]
        protected Animator animator;
        [SerializeField] protected string currentAnimName;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            animator.SetBool(idleAnimName, true);
            currentAnimName = idleAnimName;

            anim.ToEffectNotice += ToEffect;
            anim.ClearNotice += Clear;
        }

        protected void ToEffect()
        {
            ChangeAnimTo(effectAnimName);
        }

        protected void ChangeAnimTo(string _animName)
        {
            animator.SetBool(currentAnimName, false);
            currentAnimName = _animName;
            animator.SetBool(_animName, true);
        }

        protected void Clear()
        {
            animator.SetBool(currentAnimName, false);
            currentAnimName = idleAnimName;
            animator.SetBool(idleAnimName, true);
        }
    }
}