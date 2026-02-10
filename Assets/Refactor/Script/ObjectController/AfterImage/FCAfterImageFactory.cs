using EntitySystem;
using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class FCAfterImageFactory : FCObjectFactoryComponentBase
    {
        protected override void Awake()
        {
            base.Awake();
            factory.GenerateAfterImageAt += GenerateAfterImage;
        }

        protected void GenerateAfterImage(DAfterImageData _data)
        {
            GameObject newObject = pool.GetObject();
            newObject.SetActive(true);
            newObject.transform.position = _data.position;
            AAfterImage newDrop = newObject.GetComponent<AAfterImage>();
            newDrop.Setup(this, _data);
        }
    }
}
