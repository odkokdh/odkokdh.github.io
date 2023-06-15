using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
//using UnityEngine.UIElements;

public class StoreUI : MonoBehaviour
{
    public static StoreUI instance;
    public GameObject storePanel;
    bool activeStore = false;
    public bool isStoreActive;

    public ShopData shopData;
    public ShopItem[] shopItems;
    public Transform shopHolder;

    public Item item;
    public Image itemIcon;
    public bool soldOut = false;

    void Start()
    {
        storePanel.SetActive(activeStore);
        shopItems = shopHolder.GetComponentsInChildren<ShopItem>();

        for(int i = 0; i < shopItems.Length; i++)
        {
            shopItems[i].Init(this);
            shopItems[i].slotnum = i;
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            ActiveShop(activeStore);
            shopData = GetComponent<ShopData>();
            for (int i = 0; i < shopData.stocks.Count; i++)
            {
                shopItems[i].item = shopData.stocks[i];
                shopItems[i].UpdateSlotUI2();
            }
        }

        if (Input.GetKeyDown("z"))
        {
            if (activeStore)
            {
                activeStore = false;
                DeActiveShop();
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.name == "Player")
        {
            activeStore = true;
        }
    }

    public void Buy(int num)
    {
        shopData.soldOuts[num] = true;
    }
    public void ActiveShop(bool isOpen)
    {
        isStoreActive = isOpen;
        storePanel.SetActive(isOpen);

        for (int i = 0; i<32; i++)
        {
            InventoryUI.instance.slots[i].isShopMode = isOpen;
        }
    }

    public void DeActiveShop()
    {
        ActiveShop(activeStore);
        shopData = null;
        for(int i = 0; i<2; i++)
        {
            shopItems[i].RemoveSlot();
        }
    }

    public void SellBtn()
    {
        for(int i = InventoryUI.instance.slots.Length; i>0; i--)
        {
            InventoryUI.instance.slots[i-1].SellItem();
        }
    }
}
