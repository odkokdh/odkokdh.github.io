using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Weapon/AR")]

public class ItemArEft : ItemEffect
{
    public override bool ExecuteRole()
    {
        Inventory.instance.items.Add(Player.instance.rangedWeapon[0]);
        Debug.Log("AR±¸¸Å ½ºÀ§Äª");
        Player.instance.ARSwitching();
        return true;
    }
}
