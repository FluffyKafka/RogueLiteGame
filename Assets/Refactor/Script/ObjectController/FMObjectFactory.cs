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
        public Action<IItem, Vector3> GenerateDropItemAtNotice;
        public Action<DAfterImageData> GenerateAfterImageAtNotice;
        public Action<DProjectileData, Vector3> GenerateArrowAtNotice;
        public Func<float> CheckArrowGravityNotice;
        public Action<DAmmoData, Vector3> GenerateSkullAmmoAtNotice;
        public Func<DProjectileData, Vector3, GameObject> GenerateSwordAtNotice;

        public void GenerateAfterImage(DAfterImageData _data)
        {
            InvokeAction(GenerateAfterImageAtNotice, _data);
        }

        public void GenerateDropItemObject(IItem _data, Vector3 _position)
        {
            InvokeAction(GenerateDropItemAtNotice, _data, _position);
        }

        public void GenerateArrow(DProjectileData _data, Vector3 _position)
        {
            InvokeAction(GenerateArrowAtNotice, _data, _position);
        }

        public float CheckArrowGravityScale()
        {
            return InvokeFunc(CheckArrowGravityNotice);
        }
        
        public void GenerateSkullAmmo(DAmmoData _data, Vector3 _position)
        {
            InvokeAction(GenerateSkullAmmoAtNotice, _data, _position);
        }
        public GameObject GenerateSword(DProjectileData _data, Vector3 _position)
        {
            return InvokeFunc(GenerateSwordAtNotice, _data, _position);
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