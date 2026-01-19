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
            internal class AArcher : AEnemy
            {
                #region Action
                #endregion

                #region Func
                #endregion

                protected override void ComponentValidCheck()
                {
                    base.ComponentValidCheck();
                    Assert.IsNotNull(GetComponent<CArcherStateMachine>(), "缺少弓箭手状态组件");
                }
            }
        }
    }
}