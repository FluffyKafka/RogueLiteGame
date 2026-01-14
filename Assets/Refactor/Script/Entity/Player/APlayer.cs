using EntitySystem.EntityActor;
using EntitySystem.EntityActor.PlayerActor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EntitySystem
{
    namespace EntityActor
    {
        namespace PlayerActor
        {
            public interface IPlayerInput
            {
                #region Action
                public void HorizonInput(float _input);
                public void JumpInput();
                #endregion
            }

            internal class APlayer : AEntity, IPlayerInput
            {
                #region Action
                public Action<float> HorizonInput;
                public Action JumpInput;

                public Action Jump;

                public Action<float> Move;
                #endregion

                #region Func
                public Func<bool> IsGrounded_Jump;
                #endregion

                #region Input
                void IPlayerInput.HorizonInput(float _input)
                {
                    HorizonInput?.Invoke(_input);
                }
                void IPlayerInput.JumpInput()
                {
                    JumpInput?.Invoke();
                }
                #endregion
            }
        }        
    }
}
