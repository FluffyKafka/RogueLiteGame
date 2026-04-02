using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
    internal class CMinimapCamera : CUIComponentBase
    {
        private void Update()
        {
            Vector3 position = ui.CheckPlayerTransform().position;
            position.z = -1;
            transform.position = position;
        }
    }
}

