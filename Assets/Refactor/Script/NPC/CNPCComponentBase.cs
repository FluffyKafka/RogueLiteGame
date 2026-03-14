using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCSystem
{
    internal class CNPCComponentBase : MonoBehaviour
    {
        protected ANPC npc;
        protected virtual void Awake()
        {
            npc = GetComponent<ANPC>();
        }
    }
}

