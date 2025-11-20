using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;

    [Header("ToolTip")]
    public UI_ItemToolTip itemToolTip;
    public UI_StatsDescriptionToolTip statsDescriptionToolTip;
    public UI_WarningToolTip warningToolTip;
    public UI_SkillToolTip skillToolTip;
    public UI_SkillToolTip skillToolTipOuter;

    [Header("Windows")]
    public UI_CraftWindow craftWindow;

    [Header("UI")]
    [SerializeField] public GameObject charactorUI;
    [SerializeField] public GameObject skillTreeUI;
    [SerializeField] public GameObject craftUI;
    [SerializeField] public GameObject mapUI;
    [SerializeField] public GameObject optionUI;
    [SerializeField] public GameObject inGame;
    [SerializeField] public GameObject camera;

    [Header("FadeScreen")]
    [SerializeField] public UI_DarkScreen darkScreen;
    [SerializeField] private UI_DeadMessage deadMessage;
    [SerializeField] private float deadMessageDelay;

    [Header("Communcation Block")]
    [SerializeField] private UI_CommunicationBlock communicationBlock;
    [SerializeField] private UI_SkillLearningBlock skillLearningBlock;

    [Header("Trade Block")]
    [SerializeField] private UI_TradeBlock tradeBlock;
    [SerializeField] public UI_TradeWindowEquipment tradeWindowEquipment;
    [SerializeField] public UI_MaterialDetail tradeWindowMaterial;

    [Header("Scene Switch")]
    [HideInInspector] public bool isSwitching = false;

    [Header("CharacterUI")]
    [SerializeField] private TextMeshProUGUI flaskUsageTime;

    [Header("LoadingUI")]
    public GameObject loadingUIGameObject;

    [Header("Craft")]
    public UI_CraftList weaponCraftList;
    public UI_CraftList armorCraftList;
    public UI_CraftList amuletCraftList;
    public UI_CraftList flaskCraftList;

    [Header("Camera")]
    public Camera minmapCamera;
    public Camera mapCamera;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ActivateAllUI();
    }

    #region Test
    [Header("Test")]
    [SerializeField] private RectTransform skillParentSlot;
    [SerializeField] private bool isSkillTesting;
    #endregion

    void Start()
    {
        SwitchTo(inGame);
        itemToolTip.HideToolTip();
        statsDescriptionToolTip.HideToolTip();

        if(isSkillTesting)
        {
            UI_SkillSlot[] skillsTemp = skillParentSlot.GetComponentsInChildren<UI_SkillSlot>();
            foreach (var skill in skillsTemp)
            {
                skill.canbeUnlocked = true;
            }
        }

        loadingUIGameObject.SetActive(true);
    }

    void Update()
    {
        if(!darkScreen.isAnimFinish)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            SwitchWithKeyTo(charactorUI);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            SwitchWithKeyTo(skillTreeUI);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            SwitchWithKeyTo(craftUI);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            SwitchWithKeyTo(optionUI);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            SwitchWithKeyTo(mapUI);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inGame.activeSelf)
            {
                SwitchWithKeyTo(charactorUI);
            }
            else
            {
                SwitchWithKeyTo(inGame);
            }
        }
    }

    public void UpdatePlaskUsageTime(int _time)
    {
        flaskUsageTime.text = _time.ToString();
    }

    public UI_SkillToolTip GetSkillToolTip()
    {
        if(skillTreeUI.activeSelf)
        {
            return skillToolTip;
        }
        else
        {
            return skillToolTipOuter;
        }
    }

    public void HideSkillToolTip()
    {
        skillToolTip.Hide();
        skillToolTipOuter.Hide();
    }

    public void Die()
    {
        darkScreen.FadeOut();
        deadMessage.gameObject.SetActive(true);
        deadMessage.Invoke("FadeIn", deadMessageDelay);
    }

    public void ActivateAllUI()
    {
        charactorUI.SetActive(true);
        skillTreeUI.SetActive(true);
        craftUI.SetActive(true);
        optionUI.SetActive(true);
    }

    public void HideAllUI()
    {
        charactorUI.SetActive(false);
        skillTreeUI.SetActive(false);
        craftUI.SetActive(false);
        optionUI.SetActive(false);
    }

    public void SwitchTo(GameObject _menu)
    {
        mapCamera.gameObject.SetActive(true);
        minmapCamera.gameObject.SetActive(true);
        for (int i = 0; i < transform.childCount; ++i)
        {
            bool isFadeScreenOrCamera = 
                transform.GetChild(i).GetComponent<UI_DarkScreen>() != null 
                || transform.GetChild(i).GetComponentInChildren<Camera>() != null;
            if (!isFadeScreenOrCamera)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        mapCamera.gameObject.SetActive(false);
        minmapCamera.gameObject.SetActive(false);

        if (_menu != null)
        {
            _menu.SetActive(true);
        }

        if(_menu == mapUI)
        {
            mapCamera.gameObject.SetActive(true);
        }

        if(_menu == inGame)
        {
            minmapCamera.gameObject.SetActive(true);
        }

        if(GameManager.instance != null)
        {
            if(_menu != inGame)
            {
                GameManager.instance.SetPauseGame(true);
            }
            else
            {
                GameManager.instance.SetPauseGame(false);
            }
        }

        SceneAudioManager.instance.uiSFX?.buttonClick.Play(null);
    }

    private void SwitchWithKeyTo(GameObject _menu)
    {
        if (GameManager.instance.CanPause())
        {
            if (_menu != null && _menu.activeSelf)
            {
                SwitchTo(inGame);
            }
            else
            {
                SwitchTo(_menu);
            }
        }
        else
        {
            if (GameManager.instance.isBattle)
            {
                PlayerManager.instance.player.fx.CreatePopUpText("战斗中不能进行暂停");
            }
        }
    }

    public void RestartGame()
    {
        deadMessage.FadeOut();
        GameManager.instance.Invoke("RestartGame", deadMessage.fadeDuration);
    }

    public void Speak(Sentence _sentence)
    {
        if(!communicationBlock.gameObject.activeSelf)
        {
            communicationBlock.gameObject.SetActive(true);
            PlayerManager.instance.player.SetCanInput(false);
        }
        communicationBlock.Setup(_sentence);
        SceneAudioManager.instance.uiSFX.communicating.Play(null);
    }

    public void SpeakDone()
    {
        communicationBlock.gameObject.SetActive(false);
        PlayerManager.instance.player.SetCanInput(true);
    }

    public bool TryShowSkillLearningBlock()
    {
        if(skillLearningBlock.TrySetup())
        {
            skillLearningBlock.gameObject.SetActive(true);
            PlayerManager.instance.player.SetCanInput(false);

            SceneAudioManager.instance.uiSFX.buttonClick.Play(null);
            return true;
        }
        return false;
    }

    public void HideSkillLearningBlock()
    {
        skillLearningBlock.SkillChooseFinish();
        skillLearningBlock.gameObject.SetActive(false);
        PlayerManager.instance.player.SetCanInput(true);

        SceneAudioManager.instance.uiSFX.buttonClick.Play(null);
    }

    public bool IsSkillLearningBlockHide()
    {
        return !skillLearningBlock.gameObject.activeSelf;
    }

    public void ShowTradeBlock(List<ItemData> _products, List<Dialog> _successDialog, List<Dialog> _noCoinDialogs, List<Dialog> _fullBagDialogs)
    {
        tradeBlock.gameObject.SetActive(true);
        PlayerManager.instance.player.SetCanInput(false);
        tradeBlock.Setup(_products, _successDialog, _noCoinDialogs, _fullBagDialogs);

        SceneAudioManager.instance.uiSFX.buttonClick.Play(null);
    }

    public void HideTradeBlock()
    {
        tradeBlock.gameObject?.SetActive(false);
        PlayerManager.instance.player.SetCanInput(true);
    }

#if UNITY_EDITOR
    [ContextMenu("Set Craft Equipment")]
    private void SetCraftEquipment()
    {
        weaponCraftList.craftEquipment.Clear();
        armorCraftList.craftEquipment.Clear();
        amuletCraftList.craftEquipment.Clear();
        flaskCraftList.craftEquipment.Clear();
        string[] assetNames = AssetDatabase.FindAssets("t:ItemData", new[] { "Assets/Scrips/Item/ItemData" });

        foreach (string SOName in assetNames)
        {
            var SOPath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOPath);
            if (itemData is ItemData_Equipment)
            {
                var equipment = itemData as ItemData_Equipment;
                if(equipment.craftingMaterials.Count != 0)
                {
                    switch(equipment.equipmentType)
                    {
                        case EquipmentType.Weapon:
                            weaponCraftList.craftEquipment.Add(equipment); break;
                        case EquipmentType.Amulet:
                            amuletCraftList.craftEquipment.Add(equipment); break;
                        case EquipmentType.Armor:
                            armorCraftList.craftEquipment.Add(equipment); break;
                        case EquipmentType.Flask:
                            flaskCraftList.craftEquipment.Add(equipment); break;
                    }
                }
            }
        }
    }
#endif
}