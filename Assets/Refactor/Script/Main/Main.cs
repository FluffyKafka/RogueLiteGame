using EntitySystem.EntityActor.EnemyActor;
using EntitySystem.EntityActor.PlayerActor;
using InputManager;
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

        private void Awake()
        {
            inputManager.GetComponent<IInitInputManager>().Init(player.GetComponent<IInputPlayer>());
            player.GetComponent<IInitPlayer>().Init(inputManager.GetComponent<IPlayerInput>());
            enemyFactory.GetComponent<IInitEnemyFactory>().Init(player.GetComponent<IEnemyPlayer>());
        }
    }
}

