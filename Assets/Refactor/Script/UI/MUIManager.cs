using EntitySystem.EntityActor.PlayerActor;
using Item;
using StatsData;
using System.Collections;
using System.Collections.Generic;
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

        #region Init
        public void Init(IUIPlayer _player)
        {
            player = _player;
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
            throw new System.NotImplementedException();
        }

        public void MaterialStashChangeNotice(IReadOnlyList<IItem> _stash)
        {
            throw new System.NotImplementedException();
        }

        public void StashFullNotice(IItem _itemToFull)
        {
            throw new System.NotImplementedException();
        }

        public void StatsChangeNotice(DStatsData _data)
        {
            throw new System.NotImplementedException();
        }
    }
}

