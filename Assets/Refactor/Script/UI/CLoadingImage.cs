using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    internal class CLoadingImage : CUIComponentBase
    {
        private void Update()
        {
            if(ui.CheckSceneLoadRate() >= 0)
            {
                GetComponent<Scrollbar>().value = ui.CheckSceneLoadRate();
            }
        }
    }
}

