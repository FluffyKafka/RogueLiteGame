using EntitySystem.EntityActor.PlayerActor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Windows;

//需要被第一个Update
namespace InputManager
{
    public interface IInitInputManager
    {
        public void Init(IInputPlayer _player);
    }

    internal class MFixInputManager : MonoBehaviour, IInitInputManager, IPlayerInput
    {
        [SerializeField] protected IInputPlayer player;

        public float CheckHorizonInput()
        {
            return UnityEngine.Input.GetAxisRaw("Horizontal");
        }

        public float CheckVerticalInput()
        {
            return UnityEngine.Input.GetAxisRaw("Vertical");
        }

        public void Init(IInputPlayer _player)
        {
            player = _player;
        }

        //注意到更新的顺序，若两个输入事件同时发生，则前一个事件将覆盖下一个事件
        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
            {
                player.AttackInput();
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                player.JumpInput();
            }

            float xInput = UnityEngine.Input.GetAxisRaw("Horizontal");
            if (xInput != 0)
            {
                player.HorizonInput(xInput);
            }

            float yInput = UnityEngine.Input.GetAxisRaw("Vertical");
            if(yInput != 0)
            {
                player.VerticalInput(yInput);
            }
        }
    }
}

