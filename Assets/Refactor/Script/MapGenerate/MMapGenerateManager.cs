using EnemySystem;
using InventorySystem;
using NPCSystem;
using ObjectController;
using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGenerate
{
    internal enum ETileMapType
    {
        Ground,
        BackGround,
        Platform
    }

    public interface IInitMapGenerater
    {
        public void Init(
            IMapPlayer _player, IMapInventory _inventory,
            IMapEnemyFactory _enemyFactory, IMapNPCFactory _npcFactory,
            IMapObjectFactroy _objectFactory
            );
    }

    internal class MMapGenerateManager : MonoBehaviour, IInitMapGenerater
    {
        protected IMapPlayer player;
        protected IMapInventory inventory;
        protected IMapEnemyFactory enemyFactory;
        protected IMapNPCFactory npcFactory;
        protected IMapObjectFactroy objectFactory;

        public void Init(
            IMapPlayer _player, IMapInventory _inventory,
            IMapEnemyFactory _enemyFactory, IMapNPCFactory _npcFactory,
            IMapObjectFactroy _objectFactory
            )
        {
            player = _player;
            inventory = _inventory;
            enemyFactory = _enemyFactory;
            npcFactory = _npcFactory;
            objectFactory = _objectFactory;
        }
    }
}

