using ObjectGenerateData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class FCPlayerCloneFactory : FCObjectFactoryComponentBase
    {
        protected override void Awake()
        {
            base.Awake();
            factory.GeneratePlayerCloneAtNotice += GeneratePlayerCloneAt;
        }

        protected GameObject GeneratePlayerCloneAt(DPlayerCloneData _data, Vector3 _position)
        {
            GameObject newObject = pool.GetObject();
            newObject.SetActive(true);
            newObject.transform.position = _position;
            APlayerClone newDrop = newObject.GetComponent<APlayerClone>();
            newDrop.Setup(this, _data);
            return newObject;
        }
    }
}

