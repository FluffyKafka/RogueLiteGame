using AudioSystem;
using EnemySystem;
using GameManagerSystem;
using InputManager;
using InventorySystem;
using Item;
using MapGenerate;
using NPCSystem;
using ObjectController;
using PlayerSystem;
using SaveSystem;
using SkillSystem;
using StatsSystem;
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
        [SerializeField] protected GameObject objectFactory;
        [SerializeField] protected GameObject skillManager;
        [SerializeField] protected GameObject audioManager;
        [SerializeField] protected GameObject saveManager;
        [SerializeField] protected GameObject gameManager;
        [SerializeField] protected GameObject npcFactory;
        [SerializeField] protected GameObject mapGenerater;

        private void Awake()
        {
            inputManager.GetComponent<IInitInputManager>().Init(player.GetComponent<IInputPlayer>());

            player.GetComponent<IInitPlayer>().Init(
                inputManager.GetComponent<IPlayerInput>(), 
                inventory.GetComponent<IPlayerInventory>(), 
                ui.GetComponent<IPlayerUI>(), 
                objectFactory.GetComponent<IPlayerObjectFactory>(), 
                skillManager.GetComponent<IPlayerSkillManager>(),
                audioManager.GetComponent<IPlayerAudio>(),
                audioManager.GetComponent<IPlayerAudioManager>(),
                gameManager.GetComponent<IPlayerGameManager>(),
                saveManager.GetComponent<IPlayerSaveManager>()
                );

            enemyFactory.GetComponent<IInitEnemyFactory>().Init(
                player.GetComponent<IEnemyPlayer>(), 
                objectFactory.GetComponent<IEnemyObjectFactory>(),
                audioManager.GetComponents<IEnemyAduio>()
                );

            inventory.GetComponent<IInitInventory>().Init(
                equipmentFactory.GetComponent<IEquipmentFactory>(), 
                player.GetComponent<IInventoryPlayer>(), 
                itemDataBase.GetComponent<IItemDataBase>()
                );

            ui.GetComponent<IInitUI>().Init(player.GetComponent<IUIPlayer>(), audioManager.GetComponent<IUIAudio>());

            skillManager.GetComponent<IInitSkillManager>().Init(player.GetComponent<ISkillManagerPlayer>());

            audioManager.GetComponent<IInitAudio>().Init(player.GetComponent<IAudioPlayer>());

            saveManager.GetComponent<IInitSaveManager>().Init(
                player.GetComponentInChildren<ISaveStats>(),
                inventory.GetComponent<ISaveInventory>(),
                skillManager.GetComponent<ISaveSkill>(),
                audioManager.GetComponent<ISaveAduio>(),
                gameManager.GetComponent<ISaveGameManager>()
                );

            npcFactory.GetComponent<IInitNPCFactory>().Init(objectFactory.GetComponent<INPCObjectFactory>());

            gameManager.GetComponent<IInitGameManager>().Init(inputManager.GetComponent<IGameManagerInput>(), npcFactory.GetComponent<IGameManagerNPCFactory>());

            mapGenerater.GetComponent<IInitMapGenerater>().Init(
                player.GetComponent<IMapPlayer>(),
                inventory.GetComponent<IMapInventory>(),
                enemyFactory.GetComponent<IMapEnemyFactory>(),
                npcFactory.GetComponent<IMapNPCFactory>(),
                objectFactory.GetComponent<IMapObjectFactroy>(),
                inputManager.GetComponent<IMapInput>()
                );
        }
    }
}

