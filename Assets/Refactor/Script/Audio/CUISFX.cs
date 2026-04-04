using System.Collections;
using System.Collections.Generic;
using UISystem;
using UnityEngine;

namespace AudioSystem
{
    internal class CUISFX : CSoundManagerBase, IUIAudio, IMenuAudio
    {
        [Header("Sounds")]
        [SerializeField] private AudioSource buttonClick_SS;
        [SerializeField] private AudioSource craft_SS;
        [SerializeField] private AudioSource equip_SS;
        [SerializeField] private AudioSource buy_SS;
        [SerializeField] private AudioSource upgrade_SS;
        [SerializeField] private AudioSource discardInventory_SS;
        [SerializeField] private AudioSource communicating_SS;

        private TSound buttonClick;
        private TSound craft;
        private TSound equip;
        private TSound buy;
        private TSound upgrade;
        private TSound discardInventory;
        private TSound communicating;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            buttonClick = GetSound(buttonClick_SS, true);
            craft = GetSound(craft_SS, true);
            equip = GetSound(equip_SS, true);
            buy = GetSound(buy_SS, true);
            upgrade = GetSound(upgrade_SS, true);
            discardInventory = GetSound(discardInventory_SS, true);
            communicating = GetSound(communicating_SS, true);
        }

        public void ButtonClick(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                buttonClick.Play(_sourceTransform);
            }
            else
            {
                buttonClick.Stop();
            }
        }

        public void Craft(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                craft.Play(_sourceTransform);
            }
            else
            {
                craft.Stop();
            }
        }

        public void Equip(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                equip.Play(_sourceTransform);
            }
            else
            {
                equip.Stop();
            }
        }

        public void Buy(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                buy.Play(_sourceTransform);
            }
            else
            {
                buy.Stop();
            }
        }

        public void Upgrade(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                upgrade.Play(_sourceTransform);
            }
            else
            {
                upgrade.Stop();
            }
        }

        public void DiscardInventory(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                discardInventory.Play(_sourceTransform);
            }
            else
            {
                discardInventory.Stop();
            }
        }

        public void Communicating(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                communicating.Play(_sourceTransform);
            }
            else
            {
                communicating.Stop();
            }
        }
    }
}

