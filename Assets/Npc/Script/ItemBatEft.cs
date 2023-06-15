using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Weapon/Bat")]

public class ItemBatEft : ItemEffect
{
    public override bool ExecuteRole()
    {
        Inventory.instance.items.Add(Player.instance.meleeWeapon[0]);
        Player.instance.BatSwitching();
        return true;
    }
}
