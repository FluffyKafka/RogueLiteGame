using EntityBehaviour;
using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBebaviour
{
    internal class CPlayerNPCInteract : CEntityComponentBase
    {
        [SerializeField] protected float interactRadius;
        [SerializeField] protected LayerMask whatIsNPC;

        protected MPlayerBeviour player;
        protected IPlayerNPC currentInteractNPC;

        protected override void Awake()
        {
            base.Awake();
            player = entity as MPlayerBeviour;
            player.InteractToNPCNotice += Interact;
            player.CommunicateFinishNotice += CommunicateFinish;
            player.InteractFinishNotice += InteractFinish;
            player.NPCEffectFinishNotice += NPCEffectFinish;
        }

        protected void Interact()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRadius, whatIsNPC);
            foreach(var hit in hits)
            {
                currentInteractNPC = hit.GetComponent<IPlayerNPC>();
                if (currentInteractNPC != null)
                {
                    player.InteractToNPC(currentInteractNPC);
                }
            }
        }

        protected void CommunicateFinish()
        {
            currentInteractNPC.CommunicateFinish();
        }

        protected void NPCEffectFinish()
        {
            currentInteractNPC.EffectFinish();
        }

        protected void InteractFinish()
        {
            currentInteractNPC = null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, interactRadius);
        }
    }
}

