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
        public Action<DArrowData, Vector3> GenerateArrowAt;
        public Func<float> CheckArrowGravityNotice;

        public void GenerateAfterImage(DAfterImageData _data)
        {
            InvokeAction(GenerateAfterImageAt, _data);
        }

        public void GenerateDropItemObject(IItem _data, Vector3 _position)
        {
            InvokeAction(GenerateDropItemAt, _data, _position);
        }

        public void GenerateArrow(DArrowData _data, Vector3 _position)
        {
            InvokeAction(GenerateArrowAt, _data, _position);
        }

        public float CheckArrowGravityScale()
        {
            return InvokeFunc(CheckArrowGravityNotice);
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