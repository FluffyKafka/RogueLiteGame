using ObjectGenerateData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class FCSceneSwitchEntryFactory : FCObjectFactoryComponentBase
    {
        protected override void Awake()
        {
            base.Awake();
            factory.GenerateSceneSwitchEntryNotice += GenerateSceneSwitchEntry;
        }

        protected void GenerateSceneSwitchEntry(string _data, Vector3 _position)
        {
            GameObject newObject = pool.GetObject();
            newObject.SetActive(true);
            newObject.transform.position = _position;
            ASceneSwitchEntry newDrop = newObject.GetComponent<ASceneSwitchEntry>();
            newDrop.SetUp(this, _data);
        }
    }
}

