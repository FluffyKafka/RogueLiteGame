using Item;
using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class ARewordBox : AObjectController
    {
        [SerializeField] protected List<ScriptableObject> specificRewards;
        protected List<IItemData> rewards;
        protected IObjectPlayer currentInteractPlayer;
        protected bool isOpen = false; 

        public void Setup(FCRewardBoxFactory _factory, List<IItemData> _rewards)
        {
            factory = _factory;
            rewards = _rewards;
            isOpen = false;
        }

        public override bool CanInteract()
        {
            return !isOpen;
        }

        public override void Interact(IObjectPlayer _player)
        {
            base.Interact(_player);

            if(isOpen)
            {
                return;
            }

            anim.ToEffect();
            currentInteractPlayer = _player;
            isOpen = true;
        }

        public override void DamageTrigger()
        {
            DropItem();
        }

        protected void DropItem()
        {
            if (rewards == null)
            {
                foreach (var reward in specificRewards)
                {
                    currentInteractPlayer.GenerateDropItemByDataAt(reward as IItemData, transform.position);
                }
            }
            else
            {
                foreach (var reward in rewards)
                {
                    currentInteractPlayer.GenerateDropItemByDataAt(reward, transform.position);
                }
            }
        }

        public override void Clear()
        {
            base.Clear();
            rewards = null;
            currentInteractPlayer = null;
        }
    }
}

