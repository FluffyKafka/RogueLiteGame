using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public interface IEquipmentFactory
    {
        public IEquipment GetEquipment(IEquipment _equipment);
    }

    internal class FEquipmentFactory : MonoBehaviour
    {

    }
}

