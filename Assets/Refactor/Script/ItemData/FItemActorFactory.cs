using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public interface IEquipmentFactory
    {
        public IEquipment GenerateEquipment(IItemData _equipment, float initCooldown = 0);
        public IItem GenerateItem(IItemData _equipment);
    }

    internal class FItemActorFactory : MonoBehaviour, IEquipmentFactory
    {
        protected List<WItem> items = new();

        IItem IEquipmentFactory.GenerateItem(IItemData _data)
        {
            WItem newItem = new();
            newItem.Init(_data);
            items.Add(newItem);
            return newItem;
        }

        public IEquipment GenerateEquipment(IItemData _equipment, float _initCooldown = 0)
        {
            WEquipment newEquipment = new();
            newEquipment.Init(_equipment);
            newEquipment.SetCoolDownRaw(_initCooldown);
            items.Add(newEquipment);
            return newEquipment;
        }
    }
}

