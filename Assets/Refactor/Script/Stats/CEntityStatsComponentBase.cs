using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace StatsSystem
{
    internal class CEntityStatsComponentBase : MonoBehaviour
    {
        protected MEntityStatsManager statsManager;
        protected virtual void Awake()
        {
            statsManager = GetComponent<MEntityStatsManager>();
            Assert.IsNotNull(statsManager, "实体组件：" + GetType().Name + "必须附加至一个MEntityStats");
        }
    }
}

