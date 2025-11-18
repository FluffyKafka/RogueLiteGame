using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CraftList : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField] public List<ItemData_Equipment> craftEquipment;
    [SerializeField] private List<UI_CraftSlot> craftSlots;

    [SerializeField] private Color pressColor;

    void Start()
    {
        AssignCraftSlots();
    }

    private void AssignCraftSlots()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            craftSlots.Add(craftSlotParent.GetChild(i).GetComponent<UI_CraftSlot>());
        }
    }

    public void SetupCraftList()
    {
        AssignCraftSlots();

        for (int i = 0; i < craftSlots.Count; ++i)
        {
            if(craftSlots[i] != null)
            {
                Destroy(craftSlots[i].gameObject);
            }
        }

        craftSlots.Clear();
        craftSlots = new List<UI_CraftSlot>();

        for(int i = 0; i < craftEquipment.Count; ++i)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<UI_CraftSlot>().SetUpCraftSlot(craftEquipment[i]);
            craftSlots.Add(newSlot.GetComponent<UI_CraftSlot>());
        }
    }

    public void OnPointerDown(PointerEventData _eventData)
    {
        SetupCraftList();
        GetComponent<Image>().color = pressColor;
        SceneAudioManager.instance.uiSFX.buttonClick.Play(null);
    }

    public void OnPointerUp(PointerEventData _eventData)
    {
        GetComponent<Image>().color = Color.white;
    }
}
