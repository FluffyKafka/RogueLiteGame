using EntitySystem.EntityActor.EnemyActor;
using EntitySystem.EntityActor.PlayerActor;
using InputManager;
using InventorySystem;
using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class Main : MonoBehaviour
    {
        [SerializeField] protected GameObject player;
        [SerializeField] protected GameObject inputManager;
        [SerializeField] protected GameObject enemyFactory;
        [SerializeField] protected GameObject equipmentFactory;
        [SerializeField] protected GameObject inventory;

        private void Awake()
        {
            inputManager.GetComponent<IInitInputManager>().Init(player.GetComponent<IInputPlayer>());
            player.GetComponent<IInitPlayer>().Init(inputManager.GetComponent<IPlayerInput>());
            enemyFactory.GetComponent<IInitEnemyFactory>().Init(player.GetComponent<IEnemyPlayer>());
            inventory.GetComponent<IInitInventory>().Init(equipmentFactory.GetComponent<IEquipmentFactory>(), player.GetComponent<IInventoryPlayer>()); ;
        }
    }
}

