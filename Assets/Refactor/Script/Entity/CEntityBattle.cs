using EntitySystem.EntityActor;
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

