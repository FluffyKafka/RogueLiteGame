using EntityBehaviour;
using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBebaviour
{
    internal class CPlayerObjectInteract : CEntityComponentBase
    {
        [SerializeField] protected float interactRadius;
        [SerializeField] protected LayerMask whatIsObject;
        [SerializeField] protected string objectInteractClickText = "点击";
        [SerializeField] protected string objectInteractText = "进行交互";

        protected MPlayerBeviour player;
        protected bool isEnterObject = false;

        protected override void Awake()
        {
            base.Awake();
            player = entity as MPlayerBeviour;
            player.InteractToObjectNotice += Interact;
        }

        protected override void Update()
        {
            base.Update();
            if (isEnterObject)
            {
                if (!IsHitObject())
                {
                    isEnterObject = false;
                }
            }
            else
            {
                if (IsHitObject())
                {
                    isEnterObject = true;
                    player.GeneratePopUpText(objectInteractClickText + player.CheckObjectInteractInputKey() + objectInteractText);
                }
            }
        }
        protected bool IsHitObject()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRadius, whatIsObject);
            foreach (var hit in hits)
            {
                IPlayerInteractable currentInteractNPC = hit.GetComponent<IPlayerInteractable>();
                if (currentInteractNPC != null && currentInteractNPC.CanInteract())
                {
                    return true;
                }
            }
            return false;
        }

        protected void Interact()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRadius, whatIsObject);
            foreach (var hit in hits)
            {
                IPlayerInteractable currentInteractObject = hit.GetComponent<IPlayerInteractable>();
                if (currentInteractObject != null)
                {
                    player.InteractToObject(currentInteractObject);
                }
            }
        }
    }
}

