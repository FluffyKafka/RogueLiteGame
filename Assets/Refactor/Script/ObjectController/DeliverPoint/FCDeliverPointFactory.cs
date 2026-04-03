using ObjectGenerateData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class FCDeliverPointFactory : FCObjectFactoryComponentBase
    {
        protected override void Awake()
        {
            base.Awake();
            factory.GenerateDeliverPointNotice += GenerateDeliverPointAt;
        }

        protected void GenerateDeliverPointAt(Vector3 _position)
        {
            GameObject newObject = pool.GetObject();
            newObject.SetActive(true);
            newObject.transform.position = _position;
            ADeliverPoint newDrop = newObject.GetComponent<ADeliverPoint>();
            newDrop.Setup(this);
        }
    }
}

