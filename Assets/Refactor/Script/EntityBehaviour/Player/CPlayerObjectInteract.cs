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
        [SerializeField] protected List<LayerMask> whatIsObjectList;
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
                if (TryGetHitObject() == null)
                {
                    isEnterObject = false;
                }
            }
            else
            {
                IPlayerInteractable objectToInteract = TryGetHitObject();
                if (objectToInteract != null)
                {
                    isEnterObject = true;
                    if(objectToInteract.CheckInteractMessage() == string.Empty)
                    {
                        player.GeneratePopUpText(objectInteractClickText + player.CheckObjectInteractInputKey() + objectInteractText);
                    }
                    else
                    {
                        player.GeneratePopUpText(objectInteractClickText + player.CheckObjectInteractInputKey() + objectInteractText + "：" + objectToInteract.CheckInteractMessage());
                    }
                }
            }
        }
        protected IPlayerInteractable TryGetHitObject()
        {
            LayerMask whatIsObject = whatIsObjectList[0];
            for(int i = 1; i < whatIsObjectList.Count; ++i)
            {
                whatIsObject |= whatIsObjectList[i];
            }
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRadius, whatIsObject);
            foreach (var hit in hits)
            {
                IPlayerInteractable currentInteractNPC = hit.GetComponent<IPlayerInteractable>();
                if (currentInteractNPC != null && currentInteractNPC.CanInteract())
                {
                    return currentInteractNPC;
                }
            }
            return null;
        }

        protected void Interact()
        {
            LayerMask whatIsObject = whatIsObjectList[0];
            for (int i = 1; i < whatIsObjectList.Count; ++i)
            {
                whatIsObject |= whatIsObjectList[i];
            }
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

