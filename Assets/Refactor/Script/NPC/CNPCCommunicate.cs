using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCSystem
{
    internal class CNPCCommunicate : CNPCComponentBase
    {
        [SerializeField] protected int playerDialogIndex = 0;
        [SerializeField] protected int npcDialogIndex = 1;

        protected override void Awake()
        {
            base.Awake();
            npc.SetDialogIndexNotice += SetDialogIndex;
        }

        protected void SetDialogIndex(IDialog _dialog)
        {
            _dialog.SetDialogIndex(playerDialogIndex, npc.CheckCurrentInteractPlayer().GetGameObject());
            _dialog.SetDialogIndex(npcDialogIndex, npc.gameObject);
        }
    }
}

