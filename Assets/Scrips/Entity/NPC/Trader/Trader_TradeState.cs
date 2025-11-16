using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trader_TradeState : Trader_BusyState
{
    public Trader_TradeState(NPC _npcBase, NPCStateMachine _npcStateMachine, string _animBoolName, Trader _npc) : base(_npcBase, _npcStateMachine, _animBoolName, _npc)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if(npc.products.Count == 0)
        {
            List<ItemData> productList = new List<ItemData>();

            foreach (var item in npc.possibleItems)
            {
                productList.Add(item);
            }

            while (npc.products.Count < npc.productAmount && productList.Count > 0)
            {
                int randomIndex = Random.Range(0, productList.Count);
                ItemData randomProduct = productList[randomIndex];

                productList[randomIndex] = productList[productList.Count - 1];
                productList.RemoveAt(productList.Count - 1);

                npc.products.Add(randomProduct);
            }
        }
        
        UI.instance.ShowTradeBlock(npc.products, npc.successDialogs, npc.noCoinDialogs, npc.fullBagDialogs);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UI.instance.HideTradeBlock();
            stateMachine.ChangeState(npc.afterTradeState);
        }
    }
}
