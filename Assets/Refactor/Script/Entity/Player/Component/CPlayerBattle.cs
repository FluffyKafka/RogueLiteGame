using EntitySystem.EntityActor.PlayerActor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Windows;

namespace EntitySystem
{
    namespace EntityComponent
    {
        namespace BattleComponent
        {
            internal class CPlayerBattle : CEntityBattle
            {
                protected APlayer player;

                [Header("Fight Info")]
                [SerializeField] public float comboWindow;
                [SerializeField] public int comboAmount = 3;


                protected int comboCounter = 0;
                protected float lastAttackTime;

                protected override void Awake()
                {
                    base.Awake();
                    Assert.IsTrue(entity is APlayer, "CPlayerBattle组件必须附加于APlayer实体");
                    player = entity as APlayer;
                    player.AttackRaw += Attack;
                }

                protected void Attack()
                {
                    if (Time.time - lastAttackTime > comboWindow)
                    {
                        comboCounter = 0;
                    }
                    else
                    {
                        ++comboCounter;
                        comboCounter %= comboAmount;
                    }

                    lastAttackTime = Time.time;
                    player.InvokeAction(player.Attack, comboCounter);
                }
            }
        }
    }
}
