using EntitySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class FCArrowFactory : FCObjectFactoryComponentBase
    {
        protected float arrowGravity = 1f;

        protected override void Awake()
        {
            base.Awake();
            factory.GenerateArrowAt += GenerateArrow;
            factory.CheckArrowGravityNotice += CheckArrowGravity;

            GameObject newObject = pool.GetObject();
            AArrow newArrow = newObject.GetComponent<AArrow>();
            arrowGravity = newArrow.CheckGravityScale();
            RecycleObject(newArrow);
        }

        protected void GenerateArrow(DProjectileData _data, Vector3 _position)
        {
            GameObject newObject = pool.GetObject();
            newObject.SetActive(true);
            newObject.transform.position = _position;
            AArrow newDrop = newObject.GetComponent<AArrow>();
            newDrop.Setup(this, _data);
        }
        
        protected float CheckArrowGravity()
        {
            return arrowGravity;
        }
    }
}

