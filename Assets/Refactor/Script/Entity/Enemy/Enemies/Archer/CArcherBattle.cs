using EntitySystem.EntityActor.EnemyActor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EntitySystem
{
    namespace EntityComponent
    {
        namespace BattleComponent
        {
            internal class CArcherBattle : CEnemyBattle
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

