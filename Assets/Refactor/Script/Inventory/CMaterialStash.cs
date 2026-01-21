using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    internal class CMaterialStash : CInventoryComponentBase
    {
        [SerializeField] protected int stashSize;
        protected HashSet<IItemData> materials;

        protected override void Awake()
        {
            base.Awake();

            inventory.TryAddItem += TryAddStash;
        }

        protected bool TryAddStash(IItemData _equipment)
        {
            if (materials.Count >= stashSize)
            {
                return false;
            }
            else
            {
                materials.Add(_equipment);
                return true;
            }
        }
    }
}

