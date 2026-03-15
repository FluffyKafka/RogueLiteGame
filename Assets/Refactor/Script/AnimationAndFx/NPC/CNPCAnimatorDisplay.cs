using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationAndFx
{
    internal class CNPCAnimatorDisplay : CNPCComponentBase
    {
        [SerializeField] protected string idleAnimName = "Idle";
        [SerializeField] protected string effectAnimName = "Effect";

        protected Animator anim;
        protected string currentAnimName;

        protected override void Awake()
        {
            base.Awake();

            anim = GetComponent<Animator>();
            currentAnimName = idleAnimName;

            npc.IdleNotice += Idle;
            npc.EffectNotice += Effect;
        }

        protected void Idle()
        {
            ChangeTo(idleAnimName);
        }
        protected void Effect()
        {
            ChangeTo(effectAnimName);
        }

        private void ChangeTo(string _animName)
        {
            if (currentAnimName != _animName)
            {
                anim.SetBool(currentAnimName, false);
                anim.SetBool(_animName, true);
                currentAnimName = _animName;
            }        
        }
    }
}

