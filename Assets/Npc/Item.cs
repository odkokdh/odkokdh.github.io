using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType 
{ 
    Weapon,
    Consumables,
    Etc
}

public enum ItemName 
{ 
    Pistol,
    SMG,
    key,
    AR,
    SG,
    Sword,
    도끼,
    야구배트,
    bullets,
    Etc
}



[System.Serializable]
public class Item
{
    public ItemType itemType;
    public ItemName itemName;
    public int itemCost;
    public Sprite itemImage;
    public List<ItemEffect> efts;

    public bool Use()
    {
        bool isUsed = false;

        if(itemType == ItemType.Weapon)
        {
            if(itemName == ItemName.SMG)
            {
                foreach (ItemEffect eft in efts)
                {
                    isUsed = eft.ExecuteRole();
                }
            }
            else if(itemName == ItemName.SG)
            {
                foreach (ItemEffect eft in efts)
                {
                    isUsed = eft.ExecuteRole();
                }
            }
            else if(itemName == ItemName.AR)
            {
                foreach (ItemEffect eft in efts)
                {
                    isUsed = eft.ExecuteRole();
                }
            }
            else if(itemName == ItemName.도끼)
            {
                foreach (ItemEffect eft in efts)
                {
                    isUsed = eft.ExecuteRole();
                }
            }
            else if(itemName == ItemName.야구배트)
            {
                foreach (ItemEffect eft in efts)
                {
                    isUsed = eft.ExecuteRole();
                }
            }
        }
        else if(itemType == ItemType.Consumables)
        {
            if (DoorTrigger.instance.activeDoor == true)
            {
                foreach (ItemEffect eft in efts)
                {
                    isUsed = eft.ExecuteRole();
                }
            }
        }
        return isUsed;
    } 
}
