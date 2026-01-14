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

        private void Start()
        {
            inputManager.GetComponent<IInputManagerInit>().InitInputManager(player.GetComponent<IPlayerInput>());

        }
    }
}

