using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCSystem
{
    internal abstract class CNPCEffectBase : CNPCComponentBase
    {
        [SerializeField] protected bool CanRepeatInteract;
        protected bool HaveBeenInteracted;
        protected override void Awake()
        {
            base.Awake();
            npc.EffectNotice += Effect;
            npc.CanInteractNotice += CanInteract;
            npc.EffectFailNotice += InteractFail;
        }

        protected virtual void Effect()
        {
            HaveBeenInteracted = true;
        }
        protected virtual bool CanInteract()
        {
            return CanRepeatInteract || !HaveBeenInteracted;
        }
        protected void InteractFail()
        {
            HaveBeenInteracted = false;
        }
    }
}

