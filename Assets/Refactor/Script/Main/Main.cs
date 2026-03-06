using AudioSystem;
using EnemySystem;
using InputManager;
using InventorySystem;
using Item;
using PlayerSystem;
using SkillSystem;
using System.Collections;
using System.Collections.Generic;
using UISystem;
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
        [SerializeField] protected GameObject ui;
        [SerializeField] protected GameObject itemDataBase;
        [SerializeField] protected GameObject ObjectFactory;
        [SerializeField] protected GameObject skillManager;
        [SerializeField] protected GameObject audioManager;

        private void Awake()
        {
            inputManager.GetComponent<IInitInputManager>().Init(player.GetComponent<IInputPlayer>());

            player.GetComponent<IInitPlayer>().Init(
                inputManager.GetComponent<IPlayerInput>(), 
                inventory.GetComponent<IPlayerInventory>(), 
                ui.GetComponent<IPlayerUI>(), 
                ObjectFactory.GetComponent<IPlayerObjectFactory>(), 
                skillManager.GetComponent<IPlayerSkillManager>(),
                audioManager.GetComponent<IPlayerAudio>()
                );

            enemyFactory.GetComponent<IInitEnemyFactory>().Init(
                player.GetComponent<IEnemyPlayer>(), 
                ObjectFactory.GetComponent<IEnemyObjectFactory>()
                );

            inventory.GetComponent<IInitInventory>().Init(
                equipmentFactory.GetComponent<IEquipmentFactory>(), 
                player.GetComponent<IInventoryPlayer>(), 
                itemDataBase.GetComponent<IItemDataBase>()
                );

            ui.GetComponent<IInitUI>().Init(player.GetComponent<IUIPlayer>());

            skillManager.GetComponent<IInitSkillManager>().Init(player.GetComponent<ISkillManagerPlayer>());

            audioManager.GetComponent<IInitAudio>().Init(player.GetComponent<IAudioPlayer>());
        }
    }
}

