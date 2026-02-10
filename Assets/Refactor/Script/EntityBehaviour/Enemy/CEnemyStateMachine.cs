using EntityBehaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EnemyBehaviour
{
    internal abstract class CEnemyStateMachine : CEntityStateMachine
    {
        protected MEnemyBehaviour enemy;
        protected List<Coroutine> currentStateCoroutine;

        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(entity is MEnemyBehaviour, "AArcher状态机组件需要被附加至一个AArcher实体");
            enemy = entity as MEnemyBehaviour;
        }
    }
}

