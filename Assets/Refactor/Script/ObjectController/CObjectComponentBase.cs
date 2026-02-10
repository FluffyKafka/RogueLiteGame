using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class CObjectComponentBase : MonoBehaviour
    {
        protected AObjectController controller;
        protected virtual void Awake()
        {
            controller = GetComponent<AObjectController>();
        }
    }
}
