using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public interface IEquipment
    {
        public void Init(IEquipmentData _data);
        public void TryUseEffect(DEffectExcuteData _executeData);
        public bool CheckIsCoolDown();
        public float CheckCoolDownRestPer();
        public IEquipmentData CheckData();
    }
    
    internal class WEquipment : MonoBehaviour, IEquipment
    {
        protected IEquipmentData equipmentData;
        protected float coolDownTimer;

        protected void Update()
        {
            if(coolDownTimer > 0)
            {
                coolDownTimer -= Time.deltaTime;
            }
        }

        public void Init(IEquipmentData _data)
        {
            equipmentData = _data;
        }

        public void TryUseEffect(DEffectExcuteData _executeData)
        {
            if(coolDownTimer < 0)
            {
                return;
            }

            coolDownTimer = equipmentData.CheckCooldown();
            equipmentData.ExcuteItemEffect(_executeData);
        }
        public bool CheckIsCoolDown()
        {
            return coolDownTimer > 0;
        }

        public float CheckCoolDownRestPer()
        {
            if(coolDownTimer <= 0)
            {
                return 0;
            }
            else
            {
                return coolDownTimer / equipmentData.CheckCooldown();
            }
        }
        public IEquipmentData CheckData()
        {
            return equipmentData;
        }
    }
}

