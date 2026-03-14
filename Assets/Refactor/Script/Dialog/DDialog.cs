using PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogSystem
{
    [CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog")]
    internal class DDialog : ScriptableObject, IDialog
    {
        [Header("Player默认占用0号位置")]
        [SerializeField] protected List<IDialog.DSentence> dialog;
        protected Dictionary<int, GameObject> dialogIndexToEntityMap = new();

        public void SetDialogIndex(int _index, GameObject _entity)
        {
            dialogIndexToEntityMap.Add(_index, _entity);
        }

        public GameObject CheckEntityByIndex(int _index)
        {
            return dialogIndexToEntityMap[_index];
        }

        public List<IDialog.DSentence> CheckDialog()
        {
            return dialog;
        }
    }
}

