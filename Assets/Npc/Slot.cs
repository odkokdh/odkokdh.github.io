using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerUpHandler
{
    public int slotnum;
    public Item item;
    public Image itemIcon;

    public bool isShopMode;
    public bool isSell = false;
    public GameObject chkSell;

    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
    }

    public void RemoveSlot()
    {
        item = null;
        itemIcon.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(item != null)
        {
            if(!isShopMode)
            {
                bool isUse = item.Use();
                if (isUse)
                {
                    Inventory.instance.RemoveItem(slotnum);
                }
            }

            else
            {
                isSell = true;
                chkSell.SetActive(isSell);
            }
        }

    }

    public void SellItem()
    {
        if (isSell)
        {
            ItemDataBase.instance.money += item.itemCost/2;
            Inventory.instance.RemoveItem(slotnum);
            isSell = false;
            chkSell.SetActive(isSell);
        }
    }

    private void OnDisable()
    {
        isSell = false;
        chkSell.SetActive(isSell);
    }
}
