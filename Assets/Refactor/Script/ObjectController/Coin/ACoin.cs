using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

namespace ObjectController
{
    internal class ACoin : AObjectController
    {
        protected float coinAmount;
        
        public void SetUp(FCCoinFactory _factory, float _coinAmount)
        {
            factory = _factory;
            coinAmount = _coinAmount;

            InvokeAction(OriginProjectToward, 0);
            InvokeAction(ResetTrigger);
            HitPlayer += PlayerPickUp;
        }

        public override void Clear()
        {
            base.Clear();
            coinAmount = 0;
            HitPlayer -= PlayerPickUp;
        }

        protected void PlayerPickUp(IObjectPlayer _player)
        {
            InvokeAction(SecondaryProjectToward, 0);
            _player.TakeCoin(coinAmount);
            SelfRecycle();
        }
    }
}

