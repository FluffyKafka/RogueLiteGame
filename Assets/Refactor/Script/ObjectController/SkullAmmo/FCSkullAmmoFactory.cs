using EntitySystem;
using ObjectGenerateData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class FCSkullAmmoFactory : FCObjectFactoryComponentBase
    {
        protected override void Awake()
        {
            base.Awake();
            factory.GenerateSkullAmmoAtNotice += GenerateSkullAmmo;
        }

        protected void GenerateSkullAmmo(DAmmoData _data, Vector3 _position)
        {
            GameObject newObject = pool.GetObject();
            newObject.SetActive(true);
            newObject.transform.position = _position;
            ASkullAmmo newDrop = newObject.GetComponent<ASkullAmmo>();
            newDrop.Setup(this, _data);
        }
    }
}