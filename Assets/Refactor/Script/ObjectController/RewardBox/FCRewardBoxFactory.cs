using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class FCRewardBoxFactory : FCObjectFactoryComponentBase
    {
        [SerializeField] protected bool isAdvance = false;

        protected override void Awake()
        {
            base.Awake();
            if(isAdvance)
            {
                factory.GeneratePrimaryRewardBoxNotice += GenerateRewardBox;
            }
            else
            {
                factory.GenerateAdvanceRewardBoxNotice += GenerateRewardBox;
            }
        }

        protected void GenerateRewardBox(List<IItemData> _data, Vector3 _position)
        {
            GameObject newObject = pool.GetObject();
            newObject.SetActive(true);
            newObject.transform.position = _position;
            ARewordBox newDrop = newObject.GetComponent<ARewordBox>();
            newDrop.Setup(this, _data);
        }
    }
}

