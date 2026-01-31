using EnemySystem;
using Item;
using PlayerSystem;
using System;
using UnityEngine;

namespace ObjectController
{
    internal class FMObjectFactory : ComponentManagerBase, IPlayerObjectFactory, IEnemyObjectFactory
    {
        public Action<IItem, Vector3> GenerateDropItemAt;

        public void GenerateDropItemObject(IItem _data, Vector3 _position)
        {
            InvokeAction(GenerateDropItemAt, _data, _position);
        }
    }

    internal class FCObjectFactoryComponentBase : MonoBehaviour
    {
        protected FMObjectFactory factory;
        protected virtual void Awake()
        {
            factory = GetComponentInParent<FMObjectFactory>();
        }
    }
}