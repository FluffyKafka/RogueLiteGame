using PlayerSystem;
using UnityEngine;

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
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                player.AttackInput();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            { 
                player.JumpInput();
            }

            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                player.SkillInput(0);
            }

            float xInput = Input.GetAxisRaw("Horizontal");
            if (xInput != 0)
            {
                player.HorizonInput(xInput);
            }

            float yInput = Input.GetAxisRaw("Vertical");
            if(yInput != 0)
            {
                player.VerticalInput(yInput);
            }
        }
    }
}

