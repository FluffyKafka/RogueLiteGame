using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    SkillManager skill;

    [Header("HP")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    [Header("Skill CoolDown Image")]
    [SerializeField] Image dashIcon;
    [SerializeField] Image dashCoolDown;
    [Space]
    [SerializeField] Image counterAttackIcon;
    [SerializeField] Image counterAttackCoolDown;
    [Space]
    [SerializeField] Image crystalIcon;
    [SerializeField] Image crystalCoolDown;
    [Space]
    [SerializeField] Image blackHoleIcon;
    [SerializeField] Image blackHoleCoolDown;
    [Space]
    [SerializeField] Image swordThrowIcon;
    [SerializeField] Image swordThrowCoolDown;
    [Space] 
    [SerializeField] Image swordClusterIcon;
    [SerializeField] Image swordClusterCooldown;

    [Header("Equipment Cooldown Image")]
    [SerializeField] EuipmentInGameUI flaskUI;
    [SerializeField] EuipmentInGameUI weaponUI;
    [SerializeField] EuipmentInGameUI armorUI;
    [SerializeField] EuipmentInGameUI amuletUI;


    [Header("Currency")]
    [SerializeField] private TextMeshProUGUI currencyAmountText;
    [SerializeField] private float currencyAmount;
    [SerializeField] private float minIncreaseRate = 1000;
    [Range(1, 10)][SerializeField] private float increasedAlpha = 1.5f;
    [SerializeField] private TextMeshProUGUI coinAmountText;
    [SerializeField] private float coinAmount;

    private void Start()
    {
        playerStats.onCurrentHealthChange += UpdateHealthUI;
        skill = SkillManager.intance;

        dashIcon.gameObject.SetActive(skill.dash.isUnlocked_dash);
        counterAttackIcon.gameObject.SetActive(skill.counterAttack.isUnlocked_counterAttack);
        crystalIcon.gameObject.SetActive(true);
        blackHoleIcon.gameObject.SetActive(skill.blackHole.isUnlocked_blackHole);
        swordThrowIcon.gameObject.SetActive(skill.swordThrow.isUnlocked_sword);
        swordClusterIcon.gameObject.SetActive(skill.swordThrow.isUnlocked_swordCluster);
    }

    private void Update()
    {
        UpdateCurrency(currencyAmountText, ref currencyAmount, PlayerManager.instance.GetCurrencyAmount());
        UpdateCurrency(coinAmountText, ref coinAmount, PlayerManager.instance.GetCoinAmount());

        if (skill.dash.isUnlocked_dash)
        {
            dashIcon.gameObject.SetActive(true);
            dashCoolDown.fillAmount = skill.dash.GetCooldownPercentage();
        }

        if (skill.counterAttack.isUnlocked_counterAttack)
        {
            counterAttackIcon.gameObject.SetActive(true);
            counterAttackCoolDown.fillAmount = skill.counterAttack.GetCooldownPercentage();
        }

        crystalCoolDown.fillAmount = skill.crystal.GetCooldownPercentage();

        if (skill.blackHole.isUnlocked_blackHole)
        {
            blackHoleIcon.gameObject.SetActive(true);
            blackHoleCoolDown.fillAmount = skill.blackHole.GetCooldownPercentage();
        }

        if (skill.swordThrow.isUnlocked_sword)
        {
            swordThrowIcon.gameObject.SetActive(true);
            swordThrowCoolDown.fillAmount = skill.swordThrow.GetCooldownPercentage();
        }

        if (skill.swordThrow.isUnlocked_swordCluster)
        {
            swordClusterIcon.gameObject.SetActive(true);
            swordClusterCooldown.fillAmount = skill.swordThrow.GetSwordClusterCoolDownPercentage();
        }

        flaskUI.EquipmentCoolDownUpdate();
        weaponUI.EquipmentCoolDownUpdate();
        armorUI.EquipmentCoolDownUpdate();
        amuletUI.EquipmentCoolDownUpdate();
    }

    private void UpdateCurrency(TextMeshProUGUI _text, ref float _currentCurrency, float _targetCurrency)
    {
        if (Mathf.Abs(_currentCurrency - _targetCurrency) > 10)
        {
            float increaseRate = (_targetCurrency - _currentCurrency) / increasedAlpha;
            if(Mathf.Abs(increaseRate) < minIncreaseRate)
            {
                if(_currentCurrency < _targetCurrency)
                {
                    increaseRate = minIncreaseRate;
                }
                else
                {
                    increaseRate = -minIncreaseRate;
                }
            }
            _currentCurrency += increaseRate * Time.deltaTime;
        }
        else
        {
            _currentCurrency = _targetCurrency;
        }
        _text.text = ((int)_currentCurrency).ToString("#,#");
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetStatByType(StatType.MaxHealth);
        slider.value = playerStats.GetCurrentHealthValue();
    }
}
