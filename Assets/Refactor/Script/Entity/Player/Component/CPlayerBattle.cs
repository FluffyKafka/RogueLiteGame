using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem
{
    namespace EntityComponent
    {
        namespace BattleComponent
        {
            internal class CPlayerBattle : CEntityBattle
            {
                [Header("Fight Info")]
                [SerializeField] public float comboWindow;
                [SerializeField] public float movableDurationInAttacking;
                [SerializeField] public float unmovableDurationAfterAttack;
                [SerializeField] public float[] attackMovement;
            }
        }
    }
}
