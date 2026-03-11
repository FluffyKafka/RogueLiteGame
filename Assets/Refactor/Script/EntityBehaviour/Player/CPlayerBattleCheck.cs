using EntityBehaviour;
using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBebaviour
{
    internal class CPlayerBattleCheck : CEntityComponentBase
    {
        protected override void Awake()
        {
            base.Awake();
            MPlayerBeviour player = entity as MPlayerBeviour;
            player.CheckIsPlayerInBattleNotice += CheckBattle;
            player.SetPlayerToBattleNotice += SetBattle;
        }

        protected bool isBattle = false;

        protected void SetBattle(bool _isBattle)
        {
            isBattle = _isBattle;
        }

        protected bool CheckBattle()
        {
            return isBattle;
        }
    }
}

