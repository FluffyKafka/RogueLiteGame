using Item;
using PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCSystem
{
    internal class ANPC : ComponentManagerBase, IPlayerNPC, IUIDialogEntity
    {
        [SerializeField] protected ENPCType type;
        [SerializeField] protected Sprite npcIcon;
        [SerializeField] protected string npcName;

        #region ActionAndFunc
        public Action<INPCPlayer> PlayerInteractNotice;
        public Action<IDialog> SetDialogIndexNotice;
        public Action CommunicateFinishNotice;
        public Action EffectNotice;
        public Action EffectFinishNotice;
        public Action EffectFailNotice;
        public Func<bool> CanInteractNotice;
        #endregion

        protected INPCPlayer currentInteractPlayer;
        protected INPCObjectFactory objectFactory;

        protected virtual void Awake()
        {
            if(objectFactory == null)
            {
                FNPCFactory.GetInstance_TestMode().InitEnemyNotGenerateByFactory_TestMode(this, type);
            }
        }

        public void Init(INPCObjectFactory _objectFactory)
        {
            objectFactory = _objectFactory;
        }

        public ENPCType CheckType()
        {
            return type;
        }

        public void Interact(INPCPlayer _player)
        {
            currentInteractPlayer = _player;
            InvokeAction(PlayerInteractNotice, _player);
        }

        public string CheckName()
        {
            return npcName;
        }
        public Sprite CheckIcon()
        {
            return npcIcon;
        }

        public void ToCommunicate(IDialog _dialog)
        {
            currentInteractPlayer.Communicate(_dialog);
        }

        public INPCPlayer CheckCurrentInteractPlayer()
        {
            return currentInteractPlayer;
        }

        public void CommunicateFinish()
        {
            InvokeAction(CommunicateFinishNotice);
        }

        public void EffectFinish()
        {
            InvokeAction(EffectFinishNotice);
        }

        public void PlayerShowCraftPage()
        {
            currentInteractPlayer.ShowCraftPage();
        }

        public void InteractFinish()
        {
            currentInteractPlayer?.InteractFinish();
            currentInteractPlayer = null;
        }

        public float CheckPlayerSoulAmount()
        {
            return currentInteractPlayer.CheckSoulAmount();
        }
        public List<ScriptableObject> CheckPlayerCanUnlockSkillList(float _soul)
        {
            return currentInteractPlayer.CheckCanUnlockSkillList(_soul);
        }
        public void ShowSkillForSaleListToPlayer(List<DSkillForSaleToUi> _skills)
        {
            currentInteractPlayer.ShowSkillForSaleWindow(_skills);
        }

        public void AnimToIdle()
        {
            GetComponentInChildren<INPCAnimationFx>()?.Idle();
        }
        public void AnimToEffect()
        {
            GetComponentInChildren<INPCAnimationFx>()?.Effect();
        }

        public List<IItemData> CheckAllItemCanBeSale()
        {
            return currentInteractPlayer.CheckAllItemCanBeSale();
        }

        public void ShowItemForSaleToPlayerUi(List<DItemForSaleToUi> _items)
        {
            currentInteractPlayer.ShowItemForSaleToUi(_items);
        }

        public void EffectFail()
        {
            InvokeAction(EffectFailNotice);
        }
    }

    public interface INPCAnimationFx
    {
        public void Idle();
        public void Effect();
    }

    public interface INPCObjectFactory
    {
        public void GeneratePopUpText(string _data, Vector3 _position);
    }
}

