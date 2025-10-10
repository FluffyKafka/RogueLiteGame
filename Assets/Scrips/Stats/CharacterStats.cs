using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Assertions;

public enum StatType
{
    Strength,
    Agility,
    Intelligence,
    Vitality,
    MaxHealth,
    Armor,
    Evasion,
    MagicResistance,
    Damage,
    CritChance,
    CritPower,
    AttackSpeed,
    FireDamage,
    IceDamage,
    LightningDamage,
    FireDuration,
    IceDuration,
    LightningDuration,
    FireDamageCooldown,
    FireDamageTransform,
    ChillArmorReduce,
    ChillSlowRate,
    ShockAccuracyReduce,
    ThunderStrikeCount,
    ThunderStrikeRate,
    MaxFlaskUsageTime,
    FlaskUsageRecover
}
public class DamageData
{
    public Entity _damageSource { get; private set; } = null;

    public bool shouldPlayAnim { get; private set; } = true;

    public float _physical { get; private set; } = 0;
    public bool _isCrit {  get; private set; } = false;
    public float _magical { get; private set; } = 0;


    public bool _ignite { get; private set; } = false;
    public float _igniteDamageCooldown { get; private set; } = float.PositiveInfinity;
    public float _igniteDuration { get; private set; } = 0f;
    public float _igniteDamage { get; private set; } = 0f;


    public bool _chill { get; private set; } = false;
    public float _chillSlowPercentage { get; private set; } = 0f;
    public float _chillDuration { get; private set; } = 0f;
    public float _chillReduceArmorPer { get; private set; } = 20f;


    public bool _shock { get; private set; } = false;
    public float _shockDuration { get; private set; } = 0f;
    public float _thunderStrikeRadius { get; private set; } = 15f;
    public float _thunderStrikeRate { get; private set; } = 0f;
    public int _thunderStrikerCounter { get; private set; } = 0;
    public float _attackerShockReduceAccuracy { get; private set; } = 0f;



    public void SetDamageSource(Entity _damageSource)
    {
        this._damageSource = _damageSource;
    }
    public void SetShouldPlayAnim(bool _shouldPlay)
    {
        shouldPlayAnim = _shouldPlay;
    }
    public void SetPhysicsDamage(float damage, bool isCrit)
    {
        _physical = damage;
        _isCrit = isCrit;
    }
    public void SetMagicDamage(float damage)
    {
        _magical = damage;
    }
    public void SetIgnite(float igniteDamageCooldown, float igniteDuration, float igniteDamage)
    {
        _ignite = true;
        _igniteDamageCooldown = igniteDamageCooldown;
        _igniteDuration = igniteDuration;
        _igniteDamage = igniteDamage;
    }
    public void SetChill(float chillSlowPercentage, float chillDuration, float chillReduceArmarPer)
    {
        _chill = true;
        _chillSlowPercentage = chillSlowPercentage;
        _chillDuration = chillDuration;
        _chillReduceArmorPer = chillReduceArmarPer;
    }
    public void SetShock(float shockDuration, float thunderStrikeRate, int thunderStrikerCounter, float attackerShockReduceAccuracy)
    {
        _shock = true;
        _shockDuration = shockDuration;
        _thunderStrikeRate = thunderStrikeRate;
        _thunderStrikerCounter = thunderStrikerCounter;
        _attackerShockReduceAccuracy = attackerShockReduceAccuracy;
    }
}
[System.Serializable]
public class DamageDataSerializable
{
    public Entity _damageSource { get; private set; } = null;
    [SerializeField] public bool shouldPlayAnim = true;

    [SerializeField] public float _physical = -1;
    [SerializeField] public bool _isCrit = false;
    [SerializeField] public float _magical  = -1;


    [SerializeField] public bool _ignite = false;
    [SerializeField] public float _igniteDamageCooldown = float.PositiveInfinity;
    [SerializeField] public float _igniteDuration = 0f;
    [SerializeField] public float _igniteDamage = 0f;


    [SerializeField] public bool _chill = false;
    [SerializeField] public float _chillSlowPercentage = 0f;
    [SerializeField] public float _chillDuration = 0f;
    [SerializeField] public float _chillReduceArmor = 0f;


    [SerializeField] public bool _shock = false;
    [SerializeField] public float _shockDuration = 0f;
    [SerializeField] public float _thunderStrikeRadius = 15f;
    [SerializeField] public float _thunderStrikeRate = 0f;
    [SerializeField] public int _thunderStrikerCounter = 0;//雷击层数
    [SerializeField] public float _attackerShockReduceAccuracy = 0f;

    public void SetDamageSource(Entity _source)
    {
        _damageSource = _source;
    }

    public DamageData GetDamageData()
    {
        DamageData damageData = new DamageData();
        damageData.SetDamageSource(_damageSource);
        damageData.SetPhysicsDamage(_physical, _isCrit);
        damageData.SetMagicDamage(_magical);
        if(_ignite)
        {
            damageData.SetIgnite(_igniteDamageCooldown, _igniteDuration, _igniteDamage);
        }
        if(_chill)
        {
            damageData.SetChill(_chillSlowPercentage, _chillDuration, _chillReduceArmor);
        }
        if(_shock)
        {
            damageData.SetShock(_shockDuration, _thunderStrikeRate, _thunderStrikerCounter, _attackerShockReduceAccuracy);
        }
        return damageData;
    }
}

[System.Serializable]
public class StatsModifierData
{
    [Header("Major Stats")]
    public float strength;         
    public float agility;          
    public float intelligence;     
    public float vitality;         


    [Header("Defensive Stats")]
    public float maxHealth;
    public float armor;
    public float evasion;
    public float magicResistance;
    public float maxFlaskUsageTime;
    public float flaskUsageRecover;


    [Header("Offensive Stats")]
    public float damage;
    public float critChance;
    public float critPower;
    public float attackSpeed;


    [Header("Magic Stats")]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;
    public float fireDuration;
    public float iceDuration;
    public float lightningDuration;
    public float fireDamageCooldown;
    public float fireDamageTransform;
    public float chillArmorReduce;
    public float chillSlowRate;
    public float shockAccuracyReduce;
    public float thunderStrikeCount;
    public float thunderStrikeRate;

    public float GetStatByType(StatType _type)
    {
        switch (_type)
        {
            case StatType.Strength:              return strength;
            case StatType.Agility:               return agility;
            case StatType.Intelligence:          return intelligence;
            case StatType.Vitality:              return vitality;
                                                 
            case StatType.MaxHealth:             return maxHealth;
            case StatType.Armor:                 return armor;
            case StatType.Evasion:               return evasion;
            case StatType.MagicResistance:       return magicResistance;
            case StatType.MaxFlaskUsageTime:     return maxFlaskUsageTime;
            case StatType.FlaskUsageRecover: return flaskUsageRecover;
                                                 
            case StatType.Damage:                return damage;
            case StatType.CritChance:            return critChance;
            case StatType.CritPower:             return critPower;
            case StatType.AttackSpeed:           return attackSpeed;
                                                 
            case StatType.FireDamage:            return fireDamage;
            case StatType.IceDamage:             return iceDamage;
            case StatType.LightningDamage:       return lightningDamage;
            case StatType.FireDuration:          return fireDuration;
            case StatType.IceDuration:           return iceDuration;
            case StatType.LightningDuration:     return lightningDuration;

            case StatType.FireDamageCooldown:    return fireDamageCooldown;
            case StatType.FireDamageTransform:   return fireDamageTransform;
            case StatType.ChillArmorReduce:      return chillArmorReduce;
            case StatType.ChillSlowRate:         return chillSlowRate;
            case StatType.ShockAccuracyReduce:   return shockAccuracyReduce;
            case StatType.ThunderStrikeCount:    return thunderStrikeCount;
            case StatType.ThunderStrikeRate:     return thunderStrikeRate;

            default:                             return -1;
        }
    }
}

public class CharacterStats : MonoBehaviour
{
    private Entity entity;
    private EntityFX entityFX;

    [Header("Major Stats")]
    public Stat strength;                   // 1 point => increase damage by 1 and crit.power by 1%
    public Stat agility;                    // 1 point => increase evasion by 1 and crit.chance by 1%
    public Stat intelligence;               // 1 point => increase magic damage by 1 and magic resistance by 3
    public Stat vitality;                   // 1 point => increase health by 3 points
                                            
                                            
    [Header("Defensive Stats")]             
    public Stat maxHealth;                  
    public Stat armor;                      
    public Stat evasion;                    
    public Stat magicResistance;
    public Stat maxFlaskUsageTime;
    public Stat flaskUsageRecover;
                                            
                                            
    [Header("Offensive Stats")]             
    public Stat damage;                     
    public Stat critChance;                 
    public Stat critPower;                  // default value => 150%
    public Stat attackSpeed;


    [Header("Magic Stats")]
    public Stat fireDamage;      
    public Stat iceDamage;       
    public Stat lightningDamage;
    public Stat fireDuration;
    public Stat iceDuration;
    public Stat lightningDuration;
    public Stat fireDamageCooldown;
    public Stat fireDamageTransform;
    public Stat chillArmorReduce;
    public Stat chillSlowRate;
    public Stat shockAccuracyReduce;
    public Stat thunderStrikeCount;
    public Stat thunderStrikeRate;

    [Header("Default Info")]
    [SerializeField] protected float defaultCritPower = 150f;


    [Header("Stats Effect")]
    [SerializeField] protected float strength_damage = 1.0f;
    [SerializeField] protected float strength_critPower = 1.0f;
    [SerializeField] protected float agility_evasion = 1.0f; 
    [SerializeField] protected float agility_critChance = 1.0f;
    [SerializeField] protected float intelligence_magicDamage = 1.0f;
    [SerializeField] protected float intelligence_magicResistance = 3.0f;
    [SerializeField] protected float intelligence_magicDuration = 0.5f; 
    [SerializeField] protected float vitality_maxHealth = 3.0f;


    [Header("Magic Effect")]
    protected float ignitedTimer;
    protected float igniteDamageTimer;
    [Space]
    protected float chillTimer;
    [Space]
    [SerializeField] protected GameObject thunderAttackPrefab;
    [SerializeField] protected float thunderStrikeRadius = 25f;
    protected float shockTimer;


    [Header("DEBUG INFO")]
    [SerializeField] private float currentHealth;
    [SerializeField] private float igniteStateDamage = 0;
    [Space]
    [SerializeField] public bool isIgnited; // does Damage Over Time
    private float igniteDamageCooldown = float.PositiveInfinity;
    [Space]
    [SerializeField] public bool isChilled; // reduce armor 
    private float chillReduceArmorPercentage;
    [Space]
    [SerializeField] public bool isShock;   // reduce accuracy
    protected float shockReduceAccuracyPercentage;
    [Space]
    [SerializeField] protected float minFireDamageCooldown = 0.1f;

    public System.Action onCurrentHealthChange;

    public float GetMaxHealthValue()
    {
        return GetStatByType(StatType.MaxHealth);
    }

    public float GetCurrentHealthValue()
    {
        return currentHealth;
    }
    public void SetCurrentHealth(float _newCurrentHealth)
    {
        currentHealth = _newCurrentHealth;
        if (onCurrentHealthChange != null)
        {
            onCurrentHealthChange();
        }
    }


    protected virtual void Start()
    {
        entity = GetComponent<Entity>();
        entityFX = GetComponent<EntityFX>();

        critPower.SetDefaultValue(defaultCritPower);
        SetCurrentHealth(GetMaxHealthValue());
    }

    protected virtual void Update()
    {
        if (isIgnited)
        {
            ignitedTimer -= Time.deltaTime;
            igniteDamageTimer -= Time.deltaTime;

            if (ignitedTimer < 0)
            {
                isIgnited = false;
                igniteDamageCooldown = float.PositiveInfinity;
            }

            if (igniteDamageTimer < 0)
            {
                igniteDamageTimer = igniteDamageCooldown;
                SetCurrentHealth(currentHealth - igniteStateDamage);
                if(currentHealth < 0)
                {
                    entity.Die();
                }
            }
        }

        if (isChilled)
        {
            chillTimer -= Time.deltaTime;
            if (chillTimer < 0)
            {
                isChilled = false;
                chillReduceArmorPercentage = 0;
            }
        }

        if (isShock)
        {
            shockTimer -= Time.deltaTime;
            if (shockTimer < 0)
            {
                isShock = false;
                shockReduceAccuracyPercentage = 0;
            }
        }
    }

    public void StartCoroutineAfterDelay(float _delay, IEnumerator _coroutine)
    {
        StartCoroutine(StartCoroutineAfterDelay_Helper(_delay, _coroutine));
    }
    protected IEnumerator StartCoroutineAfterDelay_Helper(float _delay, IEnumerator _coroutine)
    {
        yield return new WaitForSeconds(_delay);
        StartCoroutine(_coroutine);
    }

    public void HealAfterDelay(float _delay, float _amount)
    {
        StartCoroutine(HealAfterDelay_Helper(_delay, _amount));
    }
    protected IEnumerator HealAfterDelay_Helper(float _delay, float _amount)
    {
        yield return new WaitForSeconds(_delay);
        Heal(_amount);
    }

    public virtual float DoDamageTo_PrimaryAttack(Entity _target)
    {
        DamageData damageData = new DamageData();
        damageData.SetDamageSource(entity);
        damageData.SetShouldPlayAnim(true);
        CalculatePhysicalDamage(damageData);
        CalculateCritDamage(damageData);
        CalculateMagicDamage(damageData);
        CalculateAilment(damageData);

        _target.DamageSourceNotice(entity);

        if (UnityEngine.Random.Range(0, 100) < (1 - shockReduceAccuracyPercentage))
        {
            return 0;
        }

        float realDamage = _target.GetStats().TakeDamage(damageData, transform);
        
        return realDamage;
    }
    protected virtual void CalculatePhysicalDamage(DamageData _damageData, float _damage = -1)
    {
        if(_damage < 0)
        {
            _damage = GetStatByType(StatType.Damage);
        }
        _damageData.SetPhysicsDamage(_damage, false);
    }
    protected virtual void CalculateCritDamage(DamageData _damageData, float _critChance = -1, float _critPower = -1)
    {
        if(_critChance < 0)
        {
            _critChance = GetStatByType(StatType.CritChance);
        }

        if(_critPower < 0)
        {
            _critPower = GetStatByType(StatType.CritPower);
        }

        float totalCritChance = _critChance;
        if (UnityEngine.Random.Range(0, 100) < totalCritChance)
        {
            _damageData.SetPhysicsDamage(_damageData._physical * _critPower * 0.01f, true);
        }
    }
    protected virtual void CalculateMagicDamage(DamageData _damageData, float _fire = -1, float _ice = -1, float _lightning = -1)
    {
        if(_fire < 0)
        {
            _fire = GetStatByType(StatType.FireDamage);
        }

        if(_ice < 0)
        {
            _ice = GetStatByType(StatType.IceDamage);
        }

        if(_lightning < 0)
        {
            _lightning = GetStatByType(StatType.LightningDamage);
        }

        _damageData.SetMagicDamage(
            Mathf.Max(_fire, _ice, _lightning)
        );
    }
    protected virtual void CalculateAilment(DamageData _damageData, float _fireDamage = -1, float _iceDamage = -1, float _lightningDamage = -1)
    {
        if(_fireDamage < 0)
        {
            _fireDamage = fireDamage.GetValue();
        }

        if(_iceDamage < 0)
        {
            _iceDamage = iceDamage.GetValue();
        }

        if(_lightningDamage < 0)
        {
            _lightningDamage = lightningDamage.GetValue();
        }

        float maxMagicDamage = Mathf.Max(_fireDamage, _iceDamage, _lightningDamage);

        if (maxMagicDamage == 0)
        {
            return;
        }

        bool ignite = _fireDamage == maxMagicDamage;
        bool chill = _iceDamage == maxMagicDamage;
        bool shock = _lightningDamage == maxMagicDamage;

        int sum = 0;
        while(true)
        {
            if (ignite) ++sum;
            if (chill) ++sum;
            if (shock) ++sum;
            if (sum > 1)
            {
                int rd = UnityEngine.Random.Range(0, 3);
                if (rd == 0) ignite = false;
                else if (rd == 1) chill = false;
                else if (rd == 2) shock = false;
                sum = 0;
            }
            else
            {
                break;
            }
        }

        if (ignite)
        {
            _damageData.SetIgnite(GetStatByType(StatType.FireDamageCooldown), GetStatByType(StatType.FireDuration), GetStatByType(StatType.FireDamageTransform) * 0.01f * _fireDamage);
        }
        if(chill)
        {
            _damageData.SetChill(GetStatByType(StatType.ChillSlowRate), GetStatByType(StatType.IceDuration), GetStatByType(StatType.ChillArmorReduce));
        }
        if (shock)
        {
            _damageData.SetShock(GetStatByType(StatType.LightningDuration), GetStatByType(StatType.ThunderStrikeRate), (int)GetStatByType(StatType.ThunderStrikeCount), GetStatByType(StatType.ShockAccuracyReduce));
        }
    }

    public void CalculateDamageDataWithStats(ref DamageData _damageData, bool _isMagicEffectUseStatsValue)
    {
        if (_damageData._physical < 0)
        {
            CalculatePhysicalDamage(_damageData);
        }

        if (_damageData._magical < 0)
        {
            float fire = _damageData._ignite ? -1 : 0;
            float ice = _damageData._chill ? -1 : 0;
            float shock = _damageData._shock ? -1 : 0;

            CalculateMagicDamage(_damageData, fire, ice, shock);
            if (_isMagicEffectUseStatsValue)
            {
                CalculateAilment(_damageData, fire, ice, shock);
            }
        }
        else
        {
            if (_isMagicEffectUseStatsValue)
            {
                float fire = _damageData._ignite ? _damageData._magical : 0;
                float ice = _damageData._chill ? _damageData._magical : 0;
                float shock = _damageData._shock ? _damageData._magical : 0;
                CalculateAilment(_damageData, fire, ice, shock);
            }
        }
    }


    public virtual float TakeDamage(in DamageData _damageData, Transform _damageDirection)
    {
        if (!entity.CanBeDamage())
        {
            return 0f;
        }
       
        float finalPhysicalDamage = CalculatePhysicalDamageTake(_damageData);
        float finalMagicalDamage = CalculateMagicalDamageTake(_damageData);

        float finalDamage = finalPhysicalDamage + finalMagicalDamage;
        if (finalDamage > 0)
        {
            SetCurrentHealth(currentHealth - finalDamage);
            entity.fx.CreatePopUpText(finalDamage.ToString());
            if(_damageData.shouldPlayAnim)
            {
                entity.DamageAnim(_damageDirection, finalDamage);
            }
            if(_damageData._isCrit)
            {
                entity.fx.CreateCritHitFX(_damageData._damageSource, entity, _damageDirection);
            }
            else
            {
                entity.fx.CreateHitFX(_damageData._damageSource, entity);
            }
            if (currentHealth <= 0)
            {
                entity.Die();
            }
        }

        if(finalMagicalDamage > 0)
        {
            ApplyAilment(_damageData);
        }

        return finalDamage;
    }
    protected virtual float CalculatePhysicalDamageTake(in DamageData _damageData)
    {
        if (TryAvoidAttack(_damageData))
        {
            return 0;
        }

        float finalDamage = DefenseDamage(_damageData._physical);

        return finalDamage;
    }
    protected virtual float CalculateMagicalDamageTake(in DamageData _damageData)
    {
        float finalMagicDamage = _damageData._magical - GetStatByType(StatType.MagicResistance);
        finalMagicDamage = Mathf.Clamp(_damageData._magical, 0.0f, float.MaxValue);

        return finalMagicDamage;
    }
    protected virtual void ApplyAilment(in DamageData _damageData)
    {
        if (isShock && _damageData._shock && UnityEngine.Random.Range(0, 100) < _damageData._thunderStrikeRate && _damageData._thunderStrikerCounter > 0)
        {
            if(this is EnemyStats)
            {
                Enemy nearestEnemy = TryFindNearestEnemyInRadius(_damageData._thunderStrikeRadius);

                if (nearestEnemy != null)
                {
                    ThunderStrikeController newThunder = Instantiate(thunderAttackPrefab, transform.position, Quaternion.identity).GetComponent<ThunderStrikeController>();
                    newThunder.SetUp(nearestEnemy, _damageData);
                }
            }
        }

        //当Entity已有状态时不允许附加新状态（可以进行修改）
        if (isIgnited || isChilled || isShock)
        {
            return;
        }

        isIgnited = _damageData._ignite;
        isChilled = _damageData._chill;
        isShock = _damageData._shock;

        if(isIgnited)
        {
            igniteDamageTimer = _damageData._igniteDamageCooldown;
            igniteDamageCooldown = _damageData._igniteDamageCooldown;
            ignitedTimer = _damageData._igniteDuration;
            igniteStateDamage = _damageData._igniteDamage;
            entityFX.IgniteFX(_damageData._igniteDuration);
        }
        if (isChilled)
        {
            chillTimer = _damageData._chillDuration;
            chillReduceArmorPercentage = _damageData._chillReduceArmorPer;
            entityFX.ChillFX(_damageData._chillDuration);
            entity.SlowEntityBy(_damageData._chillSlowPercentage * 0.01f, _damageData._chillDuration);
        }
        if (isShock)
        {
            shockTimer = _damageData._shockDuration;
            shockReduceAccuracyPercentage = _damageData._attackerShockReduceAccuracy;
            entityFX.ShockFX(_damageData._shockDuration);
        }
    }

    protected virtual Enemy TryFindNearestEnemyInRadius(float _radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _radius);
        float minDistance = float.PositiveInfinity;
        Enemy nearestEnemy = null;
        foreach (Collider2D hit in colliders)
        {
            Enemy enemySearch = hit.GetComponent<Enemy>();
            if (enemySearch != null && enemySearch != entity)
            {
                if (Vector2.Distance(enemySearch.transform.position, entity.transform.position) < minDistance)
                {
                    nearestEnemy = enemySearch;
                }
            }
        }

        return nearestEnemy;
    }

    protected virtual float DefenseDamage(float _damage)
    {
        float armorDefence = GetStatByType(StatType.Armor);
        _damage -= armorDefence;
        _damage = Mathf.Clamp(_damage, 0.0f, float.MaxValue);
        return _damage;
    }
    protected virtual bool TryAvoidAttack(in DamageData _damageData)
    {
        float totalEvasion = GetStatByType(StatType.Evasion);
        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }

    public virtual void AddModifier(StatsModifierData _modifierData)
    {
        float currentHealthPer = currentHealth / GetMaxHealthValue();

        strength.AddModifier(_modifierData.strength);
        agility.AddModifier(_modifierData.agility);
        intelligence.AddModifier(_modifierData.intelligence);
        vitality.AddModifier(_modifierData.vitality);

        damage.AddModifier(_modifierData.damage);
        critChance.AddModifier(_modifierData.critChance);
        critPower.AddModifier(_modifierData.critPower);
        attackSpeed.AddModifier(_modifierData.attackSpeed);

        maxHealth.AddModifier(_modifierData.maxHealth);
        armor.AddModifier(_modifierData.armor);
        evasion.AddModifier(_modifierData.evasion);
        magicResistance.AddModifier(_modifierData.magicResistance);
        flaskUsageRecover.AddModifier(_modifierData.flaskUsageRecover);
        maxFlaskUsageTime.AddModifier(_modifierData.maxFlaskUsageTime);

        fireDamage.AddModifier(_modifierData.fireDamage);
        iceDamage.AddModifier(_modifierData.iceDamage);
        lightningDamage.AddModifier(_modifierData.lightningDamage);
        fireDuration.AddModifier(_modifierData.fireDuration);
        iceDuration.AddModifier(_modifierData.iceDuration);
        lightningDuration.AddModifier(_modifierData.lightningDuration);

        fireDamageCooldown.AddModifier(_modifierData.fireDamageCooldown);
        fireDamageTransform.AddModifier(_modifierData.fireDamageTransform);
        chillArmorReduce.AddModifier(_modifierData.chillArmorReduce);
        chillSlowRate.AddModifier(_modifierData.chillSlowRate);
        shockAccuracyReduce.AddModifier(_modifierData.shockAccuracyReduce);
        thunderStrikeCount.AddModifier(_modifierData.thunderStrikeCount);
        thunderStrikeRate.AddModifier(_modifierData.thunderStrikeRate);

        SetCurrentHealth(currentHealthPer * GetMaxHealthValue());
    }
    public virtual void RemoveModifier(StatsModifierData _modifierData)
    {
        float currentHealthPer = currentHealth / GetMaxHealthValue();

        strength.RemoveModifier(_modifierData.strength);
        agility.RemoveModifier(_modifierData.agility);
        intelligence.RemoveModifier(_modifierData.intelligence);
        vitality.RemoveModifier(_modifierData.vitality);

        damage.RemoveModifier(_modifierData.damage);
        critChance.RemoveModifier(_modifierData.critChance);
        critPower.RemoveModifier(_modifierData.critPower);
        attackSpeed.RemoveModifier(_modifierData.attackSpeed);

        maxHealth.RemoveModifier(_modifierData.maxHealth);
        armor.RemoveModifier(_modifierData.armor);
        evasion.RemoveModifier(_modifierData.evasion);
        magicResistance.RemoveModifier(_modifierData.magicResistance);
        flaskUsageRecover.RemoveModifier(_modifierData.flaskUsageRecover);
        maxFlaskUsageTime.RemoveModifier(_modifierData.maxFlaskUsageTime);

        fireDamage.RemoveModifier(_modifierData.fireDamage);
        iceDamage.RemoveModifier(_modifierData.iceDamage);
        lightningDamage.RemoveModifier(_modifierData.lightningDamage);
        fireDuration.RemoveModifier(_modifierData.fireDuration);
        iceDuration.RemoveModifier(_modifierData.iceDuration);
        lightningDuration.RemoveModifier(_modifierData.lightningDuration);

        fireDamageCooldown.RemoveModifier(_modifierData.fireDamageCooldown);
        fireDamageTransform.RemoveModifier(_modifierData.fireDamageTransform);
        chillArmorReduce.RemoveModifier(_modifierData.chillArmorReduce);
        chillSlowRate.RemoveModifier(_modifierData.chillSlowRate);
        shockAccuracyReduce.RemoveModifier(_modifierData.shockAccuracyReduce);
        thunderStrikeCount.RemoveModifier(_modifierData.thunderStrikeCount);
        thunderStrikeRate.RemoveModifier(_modifierData.thunderStrikeRate);

        SetCurrentHealth(currentHealthPer * GetMaxHealthValue());
    }

    public void Heal(float _health)
    {
        currentHealth += _health;
        if(currentHealth > GetMaxHealthValue())
        {
            currentHealth = GetMaxHealthValue();
        }
        if (onCurrentHealthChange != null)
        {
            onCurrentHealthChange();
        }
    }

    public IEnumerator ModifyStatsInDurationCoroutine(StatsModifierData _modifierData, float _buffDuration)
    {
        PlayerManager.instance.player.cs.AddModifier(_modifierData);
        yield return new WaitForSeconds(_buffDuration);
        PlayerManager.instance.player.cs.RemoveModifier(_modifierData);
    }

    public float GetStatByType(StatType _type) 
    {
        switch (_type)
        {
            case StatType.Strength              : return strength.GetValue();
            case StatType.Agility               : return agility.GetValue();
            case StatType.Intelligence          : return intelligence.GetValue();
            case StatType.Vitality              : return vitality.GetValue();
                                                
            case StatType.MaxHealth             : return maxHealth.GetValue() + vitality.GetValue() * vitality_maxHealth;
            case StatType.Armor                 : return armor.GetValue() * (1 - chillReduceArmorPercentage * 0.01f);
            case StatType.Evasion               : return evasion.GetValue() + agility.GetValue() * agility_evasion;
            case StatType.MagicResistance       : return intelligence.GetValue() * intelligence_magicResistance + magicResistance.GetValue();
            case StatType.MaxFlaskUsageTime     : return maxFlaskUsageTime.GetValue() > 0 ? maxFlaskUsageTime.GetValue() : 0;
            case StatType.FlaskUsageRecover     : return flaskUsageRecover.GetValue() > 0 ? flaskUsageRecover.GetValue() : 0;
                                                
            case StatType.Damage                : return damage.GetValue() + strength.GetValue() * strength_damage;
            case StatType.CritChance            : return critChance.GetValue() + agility.GetValue() * agility_critChance;
            case StatType.CritPower             : return critPower.GetValue() + strength.GetValue() * strength_critPower;
            case StatType.AttackSpeed           : return attackSpeed.GetValue();
                                                
            case StatType.FireDamage            : return fireDamage.GetValue() + intelligence.GetValue() * intelligence_magicDamage;
            case StatType.IceDamage             : return iceDamage.GetValue() + intelligence.GetValue() * intelligence_magicDamage;
            case StatType.LightningDamage       : return lightningDamage.GetValue() + intelligence.GetValue() * intelligence_magicDamage;
            case StatType.FireDuration          : return fireDuration.GetValue() + intelligence.GetValue() * intelligence_magicDuration;
            case StatType.IceDuration           : return iceDuration.GetValue() + intelligence.GetValue() * intelligence_magicDuration;
            case StatType.LightningDuration     : return lightningDuration.GetValue() + intelligence.GetValue() * intelligence_magicDuration;

            case StatType.FireDamageCooldown    : return fireDamageCooldown.GetValue() > minFireDamageCooldown ? fireDamageCooldown.GetValue() : minFireDamageCooldown;
            case StatType.FireDamageTransform   : return fireDamageTransform.GetValue();
            case StatType.ChillArmorReduce      : return chillArmorReduce.GetValue();
            case StatType.ChillSlowRate         : return chillSlowRate.GetValue();
            case StatType.ShockAccuracyReduce   : return shockAccuracyReduce.GetValue();
            case StatType.ThunderStrikeCount    : return thunderStrikeCount.GetValue();
            case StatType.ThunderStrikeRate     : return thunderStrikeRate.GetValue();

            default                       : Assert.IsTrue(false, "Undefine StatType!"); return -1;
        }                 
    }
}