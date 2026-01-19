using EntitySystem.EntityComponent.MovementComponent;
using EntitySystem.EntityComponent.StateMachineComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace EntitySystem
{
    namespace EntityActor
    {
        namespace EnemyActor
        {
            internal class ASkeleton : AEnemy
            {
                #region Action
                #endregion

                #region Func
                #endregion

                protected override void ComponentValidCheck()
                {
                    base.ComponentValidCheck();
                    Assert.IsNotNull(GetComponent<CSkeletonStateMachine>(), "È±ÉÙ÷¼÷Ã×´Ì¬×é¼þ");
                }
            }
        }
    }
}

