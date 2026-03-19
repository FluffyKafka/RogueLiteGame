using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class FCPopUpTextFactory : FCObjectFactoryComponentBase
    {
        protected override void Awake()
        {
            base.Awake();
            factory.GeneratePopUpTextNotice += GeneratePopUpText;
        }

        protected void GeneratePopUpText(string _data, Vector3 _position)
        {
            GameObject newObject = pool.GetObject();
            newObject.SetActive(true);
            newObject.transform.position = _position;
            APopUpText newDrop = newObject.GetComponent<APopUpText>();
            newDrop.Setup(this, _data);
        }
    }
}

