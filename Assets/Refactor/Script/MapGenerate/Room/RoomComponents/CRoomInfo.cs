using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGenerate
{
    internal class CRoomInfo : MonoBehaviour
    {
        [Header("Decoration Count")]
        [SerializeField] protected int minDecorationCount;
        [SerializeField] protected int maxDecorationCount;

        public int CheckDecorationCount()
        {
            return Random.Range(minDecorationCount, maxDecorationCount);
        }
    }
}

