using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace UISystem
{
    internal class CUIComponentBase : MonoBehaviour
    {
        protected MUIManager ui;

        protected virtual void Awake()
        {
            ui = GetComponentInParent<MUIManager>();
            Assert.IsNotNull(ui, "UI组件需要附加于一个UI管理器");
        }

        protected virtual void OnEnable()
        {
            
        }

        protected virtual void OnDisable()
        {

        }
    }
}
