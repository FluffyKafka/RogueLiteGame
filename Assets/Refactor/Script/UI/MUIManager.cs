using EntitySystem.EntityActor.PlayerActor;
using Item;
using StatsData;
using System;
using System.Collections;
using System.Collections.Generic;
using UIData;
using UnityEngine;

namespace UISystem
{
    public interface IInitUI
    {
        public void Init(IUIPlayer _player);
    }

    internal class MUIManager : ComponentManagerBase, IPlayerUI, IInitUI
    {
        protected IUIPlayer player;

        #region Action
        public Action<IEquipment> Equip;
        public Action<IEquipment> UnEquip;
        public Action<IItem> dropItem;
        public Action<IReadOnlyList<IEquipment>> EquipmentStashChange;
        public Action<IReadOnlyList<IItem>> MaterialStashChange;
        public Action<WReadOnlyStatsData> StatsUpdate;
        public Action<EEquipmentType, IEquipment> EquipmentChange;
        public Action<EUIPageType> ChangePageTo;
        public Action<IEquipmentData> ShowEquipmentDetail;
        public Action<IItemData> ShowMaterialDetail;
        public Action<EStatType> ShowStatsDetail;
        public Action HideTooltip;
        #endregion

        #region Func
        public Func<int> CheckMaterialStashMaxSize;
        public Func<int> CheckEquipmentStashMaxSize;
        public Func<string, string> Translate;
        public Func<string, string> CheckKeyWordStatDescription;
        #endregion

        #region Pages
        protected class DPage
        {
            public EUIPageType type;
            public GameObject gameObject;
        }
        [SerializeField] protected List<DPage> pages;
        #endregion

        protected virtual void OnEnable()
        {
            CheckEquipmentStashMaxSize += player.CheckEquipmentStashMaxSize;
            CheckMaterialStashMaxSize += player.CheckMaterialStashMaxSize;
            ChangePageTo += ChangePageToByType;
            Equip += player.Equip;
            UnEquip += player.UnEquip;
            dropItem += player.DropItem;
        }

        protected virtual void OnDisable()
        {
            CheckEquipmentStashMaxSize -= player.CheckEquipmentStashMaxSize;
            CheckMaterialStashMaxSize -= player.CheckMaterialStashMaxSize;
            ChangePageTo += ChangePageToByType;
            Equip -= player.Equip;
            UnEquip -= player.UnEquip;
            dropItem -= player.DropItem;
        }

        #region Init
        public void Init(IUIPlayer _player)
        {
            player = _player;
        }
        #endregion

        #region Self
        protected void ChangePageToByType(EUIPageType _type)
        {
            foreach(var page in pages)
            {
                if(page.type != _type)
                {
                    page.gameObject.SetActive(false);
                }
                else
                {
                    page.gameObject.SetActive(true);
                }
            }
        }
        #endregion


        public void CraftFailNotice_LackMaterial(IReadOnlyList<IItem> _lack)
        {
            throw new System.NotImplementedException();
        }

        public void EquipmentChangeNotice(EEquipmentType _type, IEquipment _equip)
        {
            throw new System.NotImplementedException();
        }

        public void EquipmentStashChangeNotice(IReadOnlyList<IEquipment> _stash)
        {
            InvokeAction(EquipmentStashChange, _stash);
        }

        public void MaterialStashChangeNotice(IReadOnlyList<IItem> _stash)
        {
            InvokeAction(MaterialStashChange, _stash);
        }

        public void StashFullNotice(IItem _itemToFull)
        {
            throw new System.NotImplementedException();
        }

        public void StatsChangeNotice(WReadOnlyStatsData _data)
        {
            InvokeAction(StatsUpdate, _data);
        }
    }
}

