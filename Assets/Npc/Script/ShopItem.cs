using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour, IPointerUpHandler
{
    public int slotnum;
    public Item item;
    public Image itemIcon;
    public bool soldOut = false;
    public GameObject soldOutSell;

    StoreUI storeUI;

    public void Init(StoreUI Iui)
    {
        storeUI = Iui;
    }
    public void UpdateSlotUI2()
    {
        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
        
        if(soldOut)
        {
            if(item.itemType == ItemType.Weapon)
            {
                soldOutSell.SetActive(soldOut);
            }
        }
        if (!soldOut)
        {
            itemIcon.color = new Color(1f, 1f, 1f);
            soldOutSell.SetActive(soldOut);
        }

    }

    public void RemoveSlot()
    {
        item = null;
        soldOut = true;
        itemIcon.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(item != null)
        {
            if(ItemDataBase.instance.money >= item.itemCost && !soldOut && Inventory.instance.items.Count < Inventory.instance.SlotCnt)
            {
                if(item.itemType == ItemType.Consumables)
                {
                    ItemDataBase.instance.money -= item.itemCost;
                    Player.instance.bullet += 100; 
                }
                else if(item.itemType == ItemType.Weapon)
                {
                    ItemDataBase.instance.money -= item.itemCost;
                    Inventory.instance.AddItem(item);
                    soldOut = true;
                    storeUI.Buy(slotnum);
                    UpdateSlotUI2();
                }
            }
        }
    }
}
