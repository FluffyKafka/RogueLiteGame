using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCSystem
{
    internal class CBlacksmithEffect : CNPCEffectBase
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Effect()
        {
            base.Effect();
            npc.PlayerShowCraftPage();
            npc.InteractFinish();
        }
    }
}

