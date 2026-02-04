using EnemySystem;
using EntitySystem;
using Item;
using PlayerSystem;
using System;
using Tool;
using UnityEngine;

namespace ObjectController
{
    internal class FMObjectFactory : ComponentManagerBase, IPlayerObjectFactory, IEnemyObjectFactory
    {
        public Action<IItem, Vector3> GenerateDropItemAt;
        public Action<DAfterImageData> GenerateAfterImageAt;

        public void GenerateAfterImage(DAfterImageData _data)
        {
            InvokeAction(GenerateAfterImageAt, _data);
        }

        public void GenerateDropItemObject(IItem _data, Vector3 _position)
        {
            InvokeAction(GenerateDropItemAt, _data, _position);
        }
    }

    internal class FCObjectFactoryComponentBase : MonoBehaviour
    {
        protected FMObjectFactory factory;
        protected IObjectPool pool;
        [SerializeField] protected AObjectController prototype;
        [SerializeField] protected int poolInitSize;
        protected virtual void Awake()
        {
            factory = GetComponentInParent<FMObjectFactory>();
            pool = GetComponent<IObjectPool>();
            pool.InitPool(prototype.gameObject, poolInitSize);           
        }
        public void RecycleObject(AObjectController _object)
        {
            pool.RecycleObject(_object.gameObject);
            _object.Clear();
            _object.gameObject.SetActive(false);
        }
    }
}