using ObjectGenerateData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class FCSaveTorchFactory : FCObjectFactoryComponentBase
    {
        protected override void Awake()
        {
            base.Awake();
            factory.GenerateSaveTorchNotice += GenerateSaveTorch;
        }

        protected void GenerateSaveTorch(Vector3 _position)
        {
            GameObject newObject = pool.GetObject();
            newObject.SetActive(true);
            newObject.transform.position = _position;
            ASaveTorch newDrop = newObject.GetComponent<ASaveTorch>();
            newDrop.SetUp(this);
        }
    }
}

