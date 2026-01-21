using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    internal class CEquipmentStash : CInventoryComponentBase
    {
        [SerializeField] protected int stashSize;
        protected HashSet<IEquipment> equipments;

        protected override void Awake()
        {
            base.Awake();

            inventory.TryAddEquipment += TryAddStash;
        }

        protected bool TryAddStash(IEquipment _equipment)
        {
            if(equipments.Count >= stashSize)
            {
                return false;
            }
            else
            {
                equipments.Add(_equipment);
                return true;
            }
        }
    } 
}

