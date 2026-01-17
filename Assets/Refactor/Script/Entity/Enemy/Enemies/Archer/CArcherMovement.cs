using EntitySystem.EntityActor.EnemyActor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EntitySystem
{
    namespace EntityComponent
    {
        namespace MovementComponent
        {
            internal class CArcherMovement : CEnemyMovement
            {
                AArcher archer;
                protected override void Awake()
                {
                    base.Awake();
                    Assert.IsTrue(enemy is AArcher);
                    archer = enemy as AArcher;
                }
            }
        }
    }
}
