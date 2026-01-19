using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatsSystem
{
    internal class CPlayerFlaskStats : CEntityStatsComponentBase
    {
        [SerializeField] protected DStat maxFlaskUsageTime;
        [SerializeField] protected DStat flaskUsageRecover;
    }
}