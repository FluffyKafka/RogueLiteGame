using System.Collections;
using System.Collections.Generic;
using UISystem;
using Unity.VisualScripting;
using UnityEngine;

namespace Main
{
    internal class MenuMain : MonoBehaviour
    {
        [SerializeField] protected GameObject menuUi;
        [SerializeField] protected GameObject saveSystem;
        [SerializeField] protected GameObject audioSystem;
        [SerializeField] protected GameObject gameManager;

        private void Awake()
        {
            menuUi.GetComponent<IInitMenuUI>().Init(saveSystem.GetComponent<IMenuSaveManager>(), gameManager.GetComponent<IMenuGameManager>(), audioSystem.GetComponent<IMenuAudio>());
        }
    }
}

