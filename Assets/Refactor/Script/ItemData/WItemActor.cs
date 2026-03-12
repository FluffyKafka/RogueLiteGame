using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

//W系列将数据包装为一个个实体，各组件交互时传输这些实体，使用时取出实体中的数据使用
//实体将被提前制作好（除了Craft的实体）
//IItem和IEquipment为基础接口，用于数据传输
//IXXXEquipment为不同组件提供了不同的访问等级，用于组件的内部使用
namespace Item
{
    public interface IItem
    {
        public abstract IItemData CheckData();
    }

    internal class WItem: IItem
    {
        protected IItemData data;

        public virtual void Init(IItemData _data)
        {
            data = _data;
        }

        public IItemData CheckData()
        {
            return data;
        }
    }

    public interface IEquipment : IItem
    {
        public IEquipmentData CheckEquipmentData();
    }

    public interface IInventoryEquipment : IEquipment
    {
        public void Update();
        public void TryUseEffect(DEffectExcuteData _executeData);
        public bool CheckIsCoolDown();
        public float CheckCoolDownRestPer();
        public float CheckCoolDownRaw();
    }

    public interface IUIEquipment : IEquipment
    {

    }
    
    internal class WEquipment : WItem, IInventoryEquipment, IUIEquipment
    {
        protected float coolDownTimer;

        public override void Init(IItemData _data)
        {
            base.Init(_data);
            Assert.IsTrue(_data is IEquipmentData, "不能用非武器数据创建武器实例");
        }
        public void Update()
        {
            if(coolDownTimer > 0)
            {
                coolDownTimer -= Time.deltaTime;
            }
        }

        public void TryUseEffect(DEffectExcuteData _executeData)
        {
            if(coolDownTimer < 0)
            {
                return;
            }
            IEquipmentData equipmentData = data as IEquipmentData;

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
                IEquipmentData equipmentData = data as IEquipmentData;
                return coolDownTimer / equipmentData.CheckCooldown();
            }
        }
        public IEquipmentData CheckEquipmentData()
        {
            IEquipmentData equipmentData = data as IEquipmentData;
            return equipmentData;
        }

        public float CheckCoolDownRaw()
        {
            if(coolDownTimer <= 0)
            {
                return 0;
            }
            else
            {
                return coolDownTimer;
            }
        }

        public void SetCoolDownRaw(float _cooldown)
        {
            coolDownTimer = _cooldown;
        }
    }
}

