using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    internal class CCraftBlacksmithCheck : CInventoryComponentBase
    {
        [SerializeField] protected LayerMask whatIsNPC;
        [SerializeField] protected ENPCType blacksmithType;
        [SerializeField] protected float blacksmithDetachRadius;

        protected override void Awake()
        {
            base.Awake();
            inventory.CanCraftNotice_BlackSmith += CanCraft;
        }

        protected bool CanCraft()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(inventory.CheckPlayerTransform().position, blacksmithDetachRadius);
            foreach(var hit in hits)
            {
                IPlayerNPC npc = hit.GetComponent<IPlayerNPC>();
                if(npc != null && npc.CheckType() == blacksmithType)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

