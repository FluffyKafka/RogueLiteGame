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
        public Action<bool, float> GamePauseNotice;

        public void Idle()
        {
            InvokeAction(IdleNotice);
        }
        public void Effect()
        {
            InvokeAction(EffectNotice);
        }
        public void GamePause(bool _isPause, float _slowRate)
        {
            InvokeAction(GamePauseNotice, _isPause, _slowRate);
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

