using ObjectGenerateData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class FCPierceSwordFactory : FCObjectFactoryComponentBase
    {
        protected override void Awake()
        {
            base.Awake();
            factory.GeneratePierceSwordAtNotice += GeneratePierceSwordAt;
        }

        protected GameObject GeneratePierceSwordAt(DProjectileData _data, Vector3 _position)
        {
            GameObject newObject = pool.GetObject();
            newObject.SetActive(true);
            newObject.transform.position = _position;
            APierceSword newDrop = newObject.GetComponent<APierceSword>();
            newDrop.Setup(this, _data);
            return newObject;
        }
    }
}