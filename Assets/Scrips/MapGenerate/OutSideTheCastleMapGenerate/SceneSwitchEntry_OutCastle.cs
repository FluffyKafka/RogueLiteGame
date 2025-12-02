using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchEntry_OutCastle : MonoBehaviour, IPlayerEnterable
{
    [SerializeField] private string sceneName;

    [Header("PopUp Text")]
    [SerializeField] private GameObject popUpTextPrefab;
    [SerializeField] private string message;
    [SerializeField] private float lifeDuration;
    [SerializeField] private Transform popTransform;
    private PopUpText popUpText = null;

    public void Enter(Player _player)
    {
        if(sceneName == "null")
        {
            PlayerManager.instance.player.fx.CreatePopUpText("此区域暂未开放");
            return;
        }
        if(UI.instance.isSwitching)
        {
            return;
        }
        UI.instance.isSwitching = true;
        SceneLoadManager.instance.LoadSceneNamed(sceneName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            CreatePopUpText();
        }
    }
    private void CreatePopUpText()
    {
        popUpText = Instantiate(popUpTextPrefab, popTransform.position, Quaternion.identity).GetComponent<PopUpText>();

        popUpText.SetUp(message, lifeDuration);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            FinishPopUpText();
        }
    }
    private void FinishPopUpText()
    {
        popUpText.PopOut();
    }
}
