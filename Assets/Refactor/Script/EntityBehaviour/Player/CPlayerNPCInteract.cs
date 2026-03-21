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
        [SerializeField] protected string npcInteractClickText = "点击";
        [SerializeField] protected string npcInteractText = "进行交互";

        protected MPlayerBeviour player;
        protected IPlayerNPC currentInteractNPC;
        protected bool isEnterNPC = false;

        protected override void Awake()
        {
            base.Awake();
            player = entity as MPlayerBeviour;
            player.InteractToNPCNotice += Interact;
            player.CommunicateFinishNotice += CommunicateFinish;
            player.InteractFinishNotice += InteractFinish;
            player.NPCEffectFinishNotice += NPCEffectFinish;
            player.NPCEffectFailNotice += NPCEffectFail;
            isEnterNPC = false;
        }

        protected override void Update()
        {
            base.Update();
            if(isEnterNPC)
            {
                if(!IsHitNPC())
                {
                    isEnterNPC = false;
                }
            }
            else
            {
                if (IsHitNPC())
                {
                    isEnterNPC = true;
                    player.GeneratePopUpText(npcInteractClickText + player.CheckNPCInteractInputKey() + npcInteractText);
                }
            }
        }

        protected bool IsHitNPC()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRadius, whatIsNPC);
            foreach (var hit in hits)
            {
                currentInteractNPC = hit.GetComponent<IPlayerNPC>();
                if (currentInteractNPC != null)
                {
                    return true;
                }
            }
            return false;
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

        protected void NPCEffectFail()
        {
            currentInteractNPC.EffectFail();
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

