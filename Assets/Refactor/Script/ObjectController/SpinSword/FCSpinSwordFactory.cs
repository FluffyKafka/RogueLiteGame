using ObjectGenerateData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class FCSpinSwordFactory : FCObjectFactoryComponentBase
    {
        protected override void Awake()
        {
            base.Awake();
            factory.GenerateSpinSwordAtNotice += GenerateSpinSwordAt;
        }

        protected GameObject GenerateSpinSwordAt(DSpinSwordData _data, Vector3 _position)
        {
            GameObject newObject = pool.GetObject();
            newObject.SetActive(true);
            newObject.transform.position = _position;
            ASpinSword newDrop = newObject.GetComponent<ASpinSword>();
            newDrop.Setup(this, _data);
            return newObject;
        }
    }
}

