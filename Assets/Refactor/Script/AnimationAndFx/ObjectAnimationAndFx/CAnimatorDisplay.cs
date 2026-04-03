using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationAndFx
{
    internal class CAnimatorDisplay : CObjectAnimationComponentBase
    {
        [SerializeField] protected string idleAnimName = "Idle";
        [SerializeField] protected string effectAnimName = "Effect";
        [SerializeField] protected string effectTypeAnimName = "EffectType";

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
            anim.ToEffectByTypeNotice += ToEffectByType;
            anim.ClearNotice += Clear;
            anim.SetSpeedNotice += SetSpeed;
        }

        protected void ToEffect()
        {
            ChangeAnimTo(effectAnimName);
        }

        protected void ToEffectByType(int _type)
        {
            ChangeAnimTo(effectAnimName);
            animator.SetInteger(effectTypeAnimName, _type);
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

        protected void SetSpeed(float _speed)
        {
            animator.speed = _speed;
        }
    }
}