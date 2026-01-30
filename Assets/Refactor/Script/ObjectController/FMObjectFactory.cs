using EntitySystem.EntityActor;
using EntitySystem.EntityActor.EnemyActor;
using EntitySystem.EntityActor.PlayerActor;
using Item;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Tool;
using UnityEngine;
using UnityEngine.Assertions;

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