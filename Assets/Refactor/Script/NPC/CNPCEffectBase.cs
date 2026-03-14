using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCSystem
{
    internal abstract class CNPCEffectBase : CNPCComponentBase
    {
        protected override void Awake()
        {
            base.Awake();
            npc.EffectNotice += Effect;
        }

        protected abstract void Effect();
    }
}

