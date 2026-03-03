using ObjectGenerateData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class FCBounceSwordFactory : FCObjectFactoryComponentBase
    {
        protected override void Awake()
        {
            base.Awake();
            factory.GenerateBounceSwordAtNotice += GenerateBounceSwordAt;
        }

        protected GameObject GenerateBounceSwordAt(DBounceSwordData _data, Vector3 _position)
        {
            GameObject newObject = pool.GetObject();
            newObject.SetActive(true);
            newObject.transform.position = _position;
            ABounceSword newDrop = newObject.GetComponent<ABounceSword>();
            newDrop.Setup(this, _data);
            return newObject;
        }
    }
}

