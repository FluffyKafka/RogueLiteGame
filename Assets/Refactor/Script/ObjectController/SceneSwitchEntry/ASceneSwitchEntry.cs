using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace ObjectController
{
    internal class ASceneSwitchEntry : AObjectController
    {
        [SerializeField] protected string interactMessage = "Ç°Íù";

        protected string nextSceneName = string.Empty;

        public void SetUp(FCSceneSwitchEntryFactory _factory, string _nextSceneName)
        {
            nextSceneName = _nextSceneName;
        }

        public override void Interact(IObjectPlayer _player)
        {
            base.Interact(_player);
            _player.SwitchSceneTo(nextSceneName);
        }

        public override void Clear()
        {
            base.Clear();
            nextSceneName = string.Empty;
        }

        public override string CheckInteractMessage()
        {
            return interactMessage + nextSceneName;
        }
    }
}

