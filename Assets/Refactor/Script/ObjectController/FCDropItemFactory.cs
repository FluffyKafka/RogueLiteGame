using Item;
using System.Collections;
using System.Collections.Generic;
using Tool;
using UnityEngine;
using UnityEngine.Assertions;


namespace ObjectController
{
    internal class FCDropItemFactory : FCObjectFactoryComponentBase
    {
        protected IObjectPool pool;
        [SerializeField] protected ADropItem prototype;
        [SerializeField] protected int poolInitSize;

        protected override void Awake()
        {
            base.Awake();
            pool = GetComponent<IObjectPool>();
            pool.InitPool(prototype.gameObject, poolInitSize);
            factory.GenerateDropItemAt += GenerateDropItem;
        }

        protected void GenerateDropItem(IItem _data, Vector3 _position)
        {
            GameObject newObject = pool.GetObject();
            newObject.SetActive(true);
            newObject.transform.position = _position;
            ADropItem newDrop = newObject.GetComponent<ADropItem>();
            newDrop.Setup(this, _data);
        }

        public void RecycleDropItem(AObjectController _object)
        {
            if(_object is ADropItem)
            {
                pool.RecycleObject((_object as ADropItem).gameObject);
                (_object as ADropItem).Clear();
                (_object as ADropItem).gameObject.SetActive(false);
            }
        }            
    }
}

