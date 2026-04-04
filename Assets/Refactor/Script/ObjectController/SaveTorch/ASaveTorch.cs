using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class ASaveTorch : AObjectController
    {
        protected bool isFactory = false;
        public void SetUp(FCSaveTorchFactory _factory)
        {
            factory = _factory;
            HitPlayer += PlayerEnter;
            isFactory = true;
        }

        private void Start()
        {
            if(!isFactory)
            {
                SetUp(null);
            }
        }

        protected void PlayerEnter(IObjectPlayer _player)
        {
            _player.SaveGame();
            anim.ToEffect();
        }

        public override void Clear()
        {
            base.Clear();
            HitPlayer -= PlayerEnter;
        }
    }
}

