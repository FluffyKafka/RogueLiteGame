using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EntitySystem.EntityActor.AEntity;

namespace EntitySystem
{
    namespace EntityComponent
    {
        namespace BattleComponent
        {
            internal class CEntityBattle : CEntityComponentBase
            {
                [SerializeField] protected Transform attackValidCheck;
                [SerializeField] protected float attackValidCheckRadius;
                [SerializeField] protected Vector2 knockBackDir;
                [Range(0, 1)][SerializeField] protected float knockBackDirMapK = 0.5f;
                [SerializeField] protected float knockBackDuration = 0.07f;
                [SerializeField] protected bool isKnocked;

                protected bool canBeDamage_current;
                protected bool canBeDamage;

                override protected void Awake()
                {
                    base.Awake();

                    entity.CanBeDamage += CanBeDamage;
                    entity.SetCanBeDamage += SetCanBeDamage;
                }

                protected void SetCanBeDamage(CanBeDamageSetData _data)
                {
                    if(_data.isSetToDefault)
                    {
                        canBeDamage_current = canBeDamage;
                    }
                    else 
                    {
                        canBeDamage_current = _data.canBeDamage;
                        if (!_data.isTempSetting)
                        {
                            canBeDamage = _data.canBeDamage;
                        }
                    }
                }

                protected bool CanBeDamage()
                {
                    return canBeDamage_current;
                }
            }
        }
    }
}

