using Item;
using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCSystem
{
    internal class CTraderEffect : CNPCEffectBase
    {
        [SerializeField] protected int maxItemQuantity;
        [SerializeField] protected float priceMultiplier;
        protected List<DItemForSaleToUi> items = null;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Effect()
        {
            base.Effect();
            if(items == null)
            {
                items = new(maxItemQuantity);
                List<IItemData> itemOptions = npc.CheckAllItemCanBeSale();
                for(int i = 0; i < maxItemQuantity; ++i)
                {
                    IItemData item = itemOptions[Random.Range(0, itemOptions.Count)];
                    itemOptions.Remove(item);
                    items.Add(new DItemForSaleToUi(item, item.CheckPrice() * priceMultiplier));
                }
            }

            npc.ShowItemForSaleToPlayerUi(items);
        }
    }
}

