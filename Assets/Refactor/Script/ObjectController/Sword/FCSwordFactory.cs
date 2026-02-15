using EntitySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class FCSwordFactory : FCObjectFactoryComponentBase
    {
        protected override void Awake()
        {
            base.Awake();
            factory.GenerateSwordAtNotice += GenerateSwordAt;
        }

        protected GameObject GenerateSwordAt(DProjectileData _data, Vector3 _position)
        {
            GameObject newObject = pool.GetObject();
            newObject.SetActive(true);
            newObject.transform.position = _position;
            ASword newDrop = newObject.GetComponent<ASword>();
            newDrop.Setup(this, _data);
            return newObject;
        }
    }
}

