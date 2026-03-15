using NPCSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationAndFx
{
    internal class MNPCAnimationFx : ComponentManagerBase, INPCAnimationFx
    {
        public Action IdleNotice;
        public Action EffectNotice;

        public void Idle()
        {
            InvokeAction(IdleNotice);
        }
        public void Effect()
        {
            InvokeAction(EffectNotice);
        }
    }

    internal class CNPCComponentBase: MonoBehaviour
    {
        protected MNPCAnimationFx npc;

        protected virtual void Awake()
        {
            npc = GetComponent<MNPCAnimationFx>();
        }
    }
}

