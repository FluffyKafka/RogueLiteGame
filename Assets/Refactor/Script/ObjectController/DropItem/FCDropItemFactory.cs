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
        protected override void Awake()
        {
            base.Awake();
            factory.GenerateDropItemAtNotice += GenerateDropItem;
        }

        protected void GenerateDropItem(IItem _data, Vector3 _position)
        {
            GameObject newObject = pool.GetObject();
            newObject.SetActive(true);
            newObject.transform.position = _position;
            ADropItem newDrop = newObject.GetComponent<ADropItem>();
            newDrop.Setup(this, _data);
        } 
    }
}

