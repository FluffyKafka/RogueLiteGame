using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    internal class CEquipment : CInventoryComponentBase
    {
        protected IEquipment armor;
        protected IEquipment weapon;
        protected IEquipment amulet;
        protected IEquipment flask;

        protected override void Awake()
        {
            base.Awake();
        }
    }
}

