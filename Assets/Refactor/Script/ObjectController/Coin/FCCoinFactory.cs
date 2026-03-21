using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class FCCoinFactory : FCObjectFactoryComponentBase
    {
        [SerializeField] protected float singleCoinMaxAmount = 50f;

        protected override void Awake()
        {
            base.Awake();
            factory.GenerateCoinNotice += GenerateCoin;
        }

        protected void GenerateCoin(float _data, Vector3 _position)
        {
            while(_data > 0)
            {
                float coin = _data;
                if(_data > 50)
                {
                    coin = 50;
                    _data -= 50;
                }
                else
                {
                    _data = 0;
                }

                GameObject newObject = pool.GetObject();
                newObject.SetActive(true);
                newObject.transform.position = _position;
                ACoin newCoin = newObject.GetComponent<ACoin>();
                newCoin.SetUp(this, coin);
            }
        }
    }
}

